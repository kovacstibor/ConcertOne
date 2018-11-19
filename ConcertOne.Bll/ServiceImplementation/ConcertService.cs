using ConcertOne.Bll.Dto.Concert;
using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Common.ServiceInterface;
using ConcertOne.Dal.DataContext;
using ConcertOne.Dal.Entity;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.ServiceImplementation
{
    public sealed class ConcertService : IConcertService
    {
        private readonly IClock _clock;
        private readonly ConcertOneDbContext _concertOneDbContext;

        public ConcertService(
            IClock clock,
            ConcertOneDbContext concertOneDbContext )
        {
            _clock = clock ?? throw new ArgumentNullException( nameof( clock ) );
            _concertOneDbContext = concertOneDbContext ?? throw new ArgumentNullException( nameof( concertOneDbContext ) );
        }

        public async Task CreateConcertAsync(
            CreateUpdateConcertDto concert,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            if (concert.TicketLimits.Any( tl => tl.Limit < 0 ))
            {
                throw new BllException( "Ticket limits must be non-negative!" );
            }

            if (concert.TicketLimits.Any( tl => tl.UnitPrice < 0 ))
            {
                throw new BllException( "Ticket prices must be non-negative!" );
            }

            Concert newConcert = new Concert
            {
                Artist = concert.Artist,
                Description = concert.Description,
                Location = concert.Location,
                StartTime = concert.StartTime,
                CreatorId = userId,
                CreationTime = _clock.Now
            };
            foreach (TicketLimitDto limit in concert.TicketLimits.Where( tl => tl.Limit >= 0 ))
            {
                newConcert.TicketLimits.Add( new TicketLimit
                {
                    Limit = limit.Limit,
                    UnitPrice = limit.UnitPrice,
                    TicketCategoryId = limit.TicketCategoryId,
                    Concert = newConcert,
                    CreatorId = userId,
                    CreationTime = _clock.Now
                } );
            }
            foreach (string concertTag in concert.Tags)
            {
                newConcert.ConcertTags.Add( new ConcertTag
                {
                    Name = concertTag,
                    Concert = newConcert,
                    CreatorId = userId,
                    CreationTime = _clock.Now
                } );
            }

            _concertOneDbContext.Concerts.Add( newConcert );
            await _concertOneDbContext.SaveChangesAsync( cancellationToken );
        }

        public async Task DeleteConcertAsync(
            Guid concertId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            bool hasPurchasedTicket = await _concertOneDbContext.TicketPurchases
                                                .Where( tp => tp.TicketLimit.ConcertId == concertId )
                                                .AnyAsync( cancellationToken );
            if (hasPurchasedTicket)
            {
                throw new BllException( "Concert cannot be deleted, as there are purchased tickets!" );
            }

            Concert concert = await _concertOneDbContext.Concerts
                                        .Include( c => c.TicketLimits )
                                        .Include( c => c.ConcertTags )
                                        .Where( c => c.Id == concertId )
                                        .SingleOrDefaultAsync( cancellationToken );
            if (concert == null)
            {
                return;
            }

            _concertOneDbContext.TicketLimits.RemoveRange( concert.TicketLimits );
            _concertOneDbContext.ConcertTags.RemoveRange( concert.ConcertTags );
            _concertOneDbContext.Concerts.Remove( concert );
            await _concertOneDbContext.SaveChangesAsync( cancellationToken );
        }

        public async Task<ConcertDetailsDto> GetConcertDetailsAsync(
            Guid concertId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            Concert concert = await _concertOneDbContext.Concerts
                                        .AsNoTracking()
                                        .Include( c => c.ConcertTags )
                                        .Include( c => c.TicketLimits )
                                            .ThenInclude( tl => tl.TicketCategory )
                                        .Include( c => c.TicketLimits )
                                            .ThenInclude( tl => tl.TicketPurchases )
                                        .Where( c => c.Id == concertId )
                                        .SingleOrDefaultAsync( cancellationToken );
            if (concert == null)
            {
                throw new BllException( "Concert does not exists!" );
            }

            return new ConcertDetailsDto
            {
                Id = concert.Id,
                Artist = concert.Artist,
                Description = concert.Description,
                Location = concert.Location,
                StartTime = concert.StartTime,
                AvailableTickets = concert.TicketLimits.Select( tl => new TicketLimitDto
                {
                    IsAvailable = tl.TicketPurchases.Count < tl.Limit,
                    Limit = tl.Limit,
                    RemainigCount = tl.Limit - tl.TicketPurchases.Count,
                    UnitPrice = tl.UnitPrice,
                    TicketCategoryId = tl.TicketCategoryId.Value,
                    TicketCategoryName = tl.TicketCategory.Name
                } ).ToList(),
                Tags = concert.ConcertTags.Select( ct => ct.Name ).ToList()
            };
        }

        public async Task<IEnumerable<ConcertListItemDto>> GetConcertsAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            List<Concert> concerts = await _concertOneDbContext.Concerts
                                                .AsNoTracking()
                                                .Include( c => c.ConcertTags )
                                                .Include( c => c.TicketLimits )
                                                    .ThenInclude( tl => tl.TicketCategory )
                                                .ToListAsync( cancellationToken );

            return concerts.Select( c => new ConcertListItemDto
            {
                Id = c.Id,
                Artist = c.Artist,
                Location = c.Location,
                StartTime = c.StartTime,
                AvailableTickets = c.TicketLimits.ToDictionary(
                    keySelector: tl => tl.TicketCategory.Name,
                    elementSelector: tl => tl.UnitPrice ),
                Tags = c.ConcertTags.Select( ct => ct.Name ).ToList()
            } );
        }

        public async Task UpdateConcertAsync(
            Guid concertId,
            CreateUpdateConcertDto concert,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            if (concert.TicketLimits.Any( tl => tl.Limit <= 0 ))
            {
                throw new BllException( "Ticket limits must be positive!" );
            }

            if (concert.TicketLimits.Any( tl => tl.UnitPrice < 0 ))
            {
                throw new BllException( "Ticket prices must be non-negatives!" );
            }

            bool hasPurchasedTicket = await _concertOneDbContext.TicketPurchases
                                                .Where( tp => tp.TicketLimit.ConcertId == concertId )
                                                .AnyAsync( cancellationToken );
            if (hasPurchasedTicket)
            {
                throw new BllException( "Concert cannot be updated, as there are purchased tickets!" );
            }

            Concert oldConcert = await _concertOneDbContext.Concerts
                                            .Include( c => c.ConcertTags )
                                            .Include( c => c.TicketLimits )
                                            .Where( c => c.Id == concertId )
                                            .SingleAsync( cancellationToken );

            _concertOneDbContext.TicketLimits.RemoveRange( oldConcert.TicketLimits );
            _concertOneDbContext.ConcertTags.RemoveRange( oldConcert.ConcertTags );

            oldConcert.Artist = concert.Artist;
            oldConcert.Description = concert.Description;
            oldConcert.Location = concert.Location;
            oldConcert.StartTime = concert.StartTime;
            oldConcert.LastModifierId = userId;
            oldConcert.LastModificationTime = _clock.Now;
            oldConcert.ConcertTags.Clear();
            oldConcert.TicketLimits.Clear();
            foreach (TicketLimitDto limit in concert.TicketLimits)
            {
                oldConcert.TicketLimits.Add( new TicketLimit
                {
                    Limit = limit.Limit,
                    UnitPrice = limit.UnitPrice,
                    TicketCategoryId = limit.TicketCategoryId,
                    Concert = oldConcert,
                    CreatorId = userId,
                    CreationTime = _clock.Now
                } );
            }
            foreach (string concertTag in concert.Tags)
            {
                oldConcert.ConcertTags.Add( new ConcertTag
                {
                    Name = concertTag,
                    Concert = oldConcert,
                    CreatorId = userId,
                    CreationTime = _clock.Now
                } );
            }

            await _concertOneDbContext.SaveChangesAsync( cancellationToken );
        }
    }
}
