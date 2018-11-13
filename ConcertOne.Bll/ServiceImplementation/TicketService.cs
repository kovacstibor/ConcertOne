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
    public class TicketService : ITicketService
    {
        private readonly IClock _clock;
        private readonly ConcertOneDbContext _concertOneDbContext;

        public TicketService(
            IClock clock,
            ConcertOneDbContext concertOneDbContext )
        {
            _clock = clock ?? throw new ArgumentNullException( nameof( clock ) );
            _concertOneDbContext = concertOneDbContext ?? throw new ArgumentNullException( nameof( concertOneDbContext ) );
        }

        public async Task<IEnumerable<Ticket>> GetPurchasedTickets(
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            return await _concertOneDbContext.Tickets
                            .AsNoTracking()
                            .Include( t => t.TicketCategory )
                            .Include( t => t.TicketPurchase )
                                .ThenInclude( tp => tp.Concert )
                            .Where( t => t.TicketPurchase.UserId == userId )
                            .ToListAsync( cancellationToken );
        }

        public async Task PurchaseTicketAsync(
            Guid userId,
            Guid concertId,
            Dictionary<Guid, int> purchases,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            if (purchases.Any( p => p.Value <= 0 ))
            {
                throw new BllException( "Purchased ticket counts must be positive" );
            }

            TicketPurchase newTicketPurchase = new TicketPurchase
            {
                ConcertId = concertId,
                UserId = userId,
                CreatorId = userId,
                CreationTime = _clock.Now
            };

            foreach (KeyValuePair<Guid, int> ticketPurchase in purchases)
            {
                int maxPurchaseCount = await _concertOneDbContext.TicketLimits
                                                .Where( tl => tl.TicketCategoryId == ticketPurchase.Key )
                                                .Where( tl => tl.ConcertId == concertId )
                                                .Select( tl => tl.Limit )
                                                .SingleAsync( cancellationToken );
                int currentPurchaseCount = await _concertOneDbContext.Tickets
                                                    .Where( t => t.TicketCategoryId == ticketPurchase.Key )
                                                    .Where( t => t.TicketPurchase.ConcertId == concertId )
                                                    .CountAsync( cancellationToken );

                if (currentPurchaseCount + ticketPurchase.Value > maxPurchaseCount)
                {
                    throw new BllException( "Ticket limit is exceeded!" );
                }

                for (int i = 0; i < ticketPurchase.Value; ++i)
                {
                    Ticket newTicket = new Ticket
                    {
                        TicketPurchase = newTicketPurchase,
                        TicketCategoryId = ticketPurchase.Key,
                        CreatorId = userId,
                        CreationTime = _clock.Now
                    };
                    newTicketPurchase.Tickets.Add( newTicket );
                }
            }

            _concertOneDbContext.TicketPurchases.Add( newTicketPurchase );

            try
            {
                await _concertOneDbContext.SaveChangesAsync( cancellationToken );
            }
            catch
            {
                throw new BllException( "Error while purchasing" );
            }
        }
    }
}
