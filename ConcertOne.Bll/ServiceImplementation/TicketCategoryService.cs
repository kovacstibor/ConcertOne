using ConcertOne.Bll.Dto;
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

        public async Task CreateTicketCategoryAsync(
            TicketCategoryDataDto ticketCategory,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            TicketCategory newTicketCategory = new TicketCategory
            {
                Name = ticketCategory.Name,
                UnitPrice = ticketCategory.UnitPrice,
                Monetary = ticketCategory.Monetary,
                CreatorId = userId,
                CreationTime = _clock.Now
            };

            _concertOneDbContext.TicketCategories.Add( newTicketCategory );

            try
            {
                await _concertOneDbContext.SaveChangesAsync( cancellationToken );
            }
            catch
            {
                throw new BllException( "Error while creating the ticket category!" );
            }
        }

        public async Task DeleteTicketCategoryAsync(
            Guid ticketCategoryId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            TicketCategory ticketCategoryToDelete = await _concertOneDbContext.TicketCategories
                                                            .Include( tc => tc.TicketLimits )
                                                            .Where( tc => tc.Id == ticketCategoryId )
                                                            .SingleOrDefaultAsync( cancellationToken );
            bool hasPurchasedTicket = await _concertOneDbContext.Tickets
                                              .Where( t => t.TicketCategoryId == ticketCategoryId )
                                              .AnyAsync( cancellationToken );

            if (ticketCategoryToDelete == null)
            {
                throw new BllException( "Ticket category not found!" );
            }

            if (hasPurchasedTicket)
            {
                throw new BllException( "Ticket category with purchased ticket cannot be deleted!" );
            }

            _concertOneDbContext.TicketLimits.RemoveRange( ticketCategoryToDelete.TicketLimits );
            _concertOneDbContext.TicketCategories.Remove( ticketCategoryToDelete );
            try
            {
                await _concertOneDbContext.SaveChangesAsync( cancellationToken );
            }
            catch
            {
                new BllException( "Error while deleting a ticket category!" );
            }
        }

        public async Task<IEnumerable<TicketCategory>> GetTicketCategoriesAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            return await _concertOneDbContext.TicketCategories
                            .AsNoTracking()
                            .ToListAsync( cancellationToken );
        }
    }
}
