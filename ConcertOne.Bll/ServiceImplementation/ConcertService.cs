using ConcertOne.Bll.Dto;
using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Common.ServiceInterface;
using ConcertOne.Dal.DataContext;
using ConcertOne.Dal.Entity;
using ConcertOne.Dal.Identity;
using Microsoft.AspNetCore.Identity;
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
            ConcertOneDbContext concertOneDbContext)
        {
            _clock = clock ?? throw new ArgumentNullException( nameof( clock ) );
            _concertOneDbContext = concertOneDbContext ?? throw new ArgumentNullException( nameof( concertOneDbContext ) );
        }

        public async Task CreateConcertAsync(
            ConcertDataDto concert,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            if (concert.AvailableTickets.Any( at => at.Value <= 0 ))
            {
                throw new BllException( "Ticket limits must be positive!" );
            }

            Concert newConcert = new Concert
            {
                Artist = concert.Artist,
                AttachedImageUrl = concert.AttachedImageUrl,
                Description = concert.Description,
                Location = concert.Location,
                StartTime = concert.StartTime,
                CreatorId = userId,
                CreationTime = _clock.Now
            };
            newConcert.TicketLimits = concert.AvailableTickets.Select( at => new TicketLimit
            {
                Concert = newConcert,
                Limit = at.Value,
                TicketCategoryId = at.Key,
                CreatorId = userId,
                CreationTime = _clock.Now
            } ).ToList();

            _concertOneDbContext.Concerts.Add( newConcert );

            try
            {
                await _concertOneDbContext.SaveChangesAsync( cancellationToken );
            }
            catch
            {
                throw new BllException( "Error while creating the concert!" );
            }
        }

        public async Task DeleteConcertAsync(
            Guid concertId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            Concert concertToDelete = await _concertOneDbContext.Concerts
                                                .Include( c => c.TicketLimits )
                                                .Include( c => c.TicketPurchases )
                                                .Where( c => c.Id == concertId )
                                                .SingleOrDefaultAsync( cancellationToken );

            if (concertToDelete == null)
            {
                throw new BllException( "Concert not found!" );
            }

            if (concertToDelete.TicketPurchases.Any())
            {
                throw new BllException( "Concert with purchased ticket cannot be deleted!" );
            }

            _concertOneDbContext.TicketLimits.RemoveRange( concertToDelete.TicketLimits );
            _concertOneDbContext.Concerts.Remove( concertToDelete );
            try
            {
                await _concertOneDbContext.SaveChangesAsync( cancellationToken );
            }
            catch
            {
                new BllException( "Error while deleting a concert!" );
            }
        }

        public async Task<Concert> GetConcertDetailsAsync(
            Guid concertId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            return await _concertOneDbContext.Concerts
                            .AsNoTracking()
                            .Include( c => c.TicketLimits )
                                .ThenInclude( tl => tl.TicketCategory )
                            .Where( c => c.Id == concertId )
                            .SingleOrDefaultAsync( cancellationToken );
        }

        public async Task<IEnumerable<Concert>> GetConcertsAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            return await _concertOneDbContext.Concerts
                            .AsNoTracking()
                            .ToListAsync( cancellationToken );
        }

        public async Task UpdateConcertAsync(
            Guid concertId,
            ConcertDataDto modifiedConcert,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            if (modifiedConcert.AvailableTickets.Any( at => at.Value <= 0 ))
            {
                throw new BllException( "Ticket limits must be positive!" );
            }

            Concert concertToModify = await _concertOneDbContext.Concerts
                                                .Include( c => c.TicketLimits )
                                                .Where( c => c.Id == concertId )
                                                .SingleAsync( cancellationToken );

            _concertOneDbContext.TicketLimits.RemoveRange( concertToModify.TicketLimits );
            _concertOneDbContext.TicketLimits.AddRange( modifiedConcert.AvailableTickets.Select( at => new TicketLimit
            {
                Concert = concertToModify,
                Limit = at.Value,
                TicketCategoryId = at.Key,
                CreatorId = userId,
                CreationTime = _clock.Now,
            } ) );
            concertToModify.LastModificationTime = _clock.Now;
            concertToModify.LastModifierId = userId;

            try
            {
                await _concertOneDbContext.SaveChangesAsync( cancellationToken );
            }
            catch
            {
                throw new BllException( "Error while updating a concert!" );
            }
        }
    }
}
