using ConcertOne.Bll.Dto.TicketCategory;
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
    public class TicketCategoryService : ITicketCategoryService
    {
        private readonly IClock _clock;
        private readonly ConcertOneDbContext _concertOneDbContext;

        public TicketCategoryService(
            IClock clock,
            ConcertOneDbContext concertOneDbContext )
        {
            _clock = clock ?? throw new ArgumentNullException( nameof( clock ) );
            _concertOneDbContext = concertOneDbContext ?? throw new ArgumentNullException( nameof( concertOneDbContext ) );
        }

        public async Task<IEnumerable<string>> GetTicketCategoriesAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            return await _concertOneDbContext.TicketCategories
                            .AsNoTracking()
                            .Select( tc => tc.Name )
                            .ToListAsync( cancellationToken );
        }

        public async Task<IEnumerable<TicketCategoryListItemDto>> GetTicketCategoriesWithIdsAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            List<TicketCategory> ticketCategories = await _concertOneDbContext.TicketCategories
                                                            .AsNoTracking()
                                                            .ToListAsync( cancellationToken );
            return ticketCategories.Select( tc => new TicketCategoryListItemDto
            {
                Id = tc.Id,
                Name = tc.Name
            } );
        }

        public async Task UpdateTicketCategoriesAsync(
            List<string> ticketCategories,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            List<TicketCategory> currentTicketCategories = await _concertOneDbContext.TicketCategories
                                                                    .Include( tc => tc.TicketLimits )
                                                                        .ThenInclude( tl => tl.TicketPurchases )
                                                                    .ToListAsync( cancellationToken );

            foreach (TicketCategory currentTicketCategory in currentTicketCategories)
            {
                if (!ticketCategories.Contains( currentTicketCategory.Name ))
                {
                    if (currentTicketCategory.TicketLimits.Any( tl => tl.TicketPurchases.Any() ))
                    {
                        throw new BllException( "Ticket category cannot be deleted, as tickets are already purchased!" );
                    }
                    else
                    {
                        _concertOneDbContext.TicketLimits.RemoveRange( currentTicketCategory.TicketLimits );
                        _concertOneDbContext.TicketCategories.Remove( currentTicketCategory );
                    }
                }
            }

            foreach (string ticketCategory in ticketCategories)
            {
                if (!currentTicketCategories.Any( ctc => ctc.Name == ticketCategory ))
                {
                    _concertOneDbContext.TicketCategories.Add( new TicketCategory
                    {
                        Name = ticketCategory,
                        CreatorId = userId,
                        CreationTime = _clock.Now
                    } );
                }
            }

            await _concertOneDbContext.SaveChangesAsync( cancellationToken );
        }
    }
}
