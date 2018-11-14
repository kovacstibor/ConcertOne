using ConcertOne.Bll.Dto.TicketPurchase;
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

        public async Task PurchaseTicketAsync(
            Guid concertId,
            Guid ticketCategoryId,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            TicketLimit ticketLimit = await _concertOneDbContext.TicketLimits
                                                .Include( tl => tl.TicketPurchases )
                                                .Where( tl => tl.ConcertId == concertId )
                                                .Where( tl => tl.TicketCategoryId == ticketCategoryId )
                                                .SingleOrDefaultAsync( cancellationToken );
            if (ticketLimit == null)
            {
                throw new BllException( "Ticket limit not found!" );
            }

            if (ticketLimit.TicketPurchases.Count >= ticketLimit.Limit)
            {
                throw new BllException( "Tickets sold out to this ticket category!" );
            }

            ticketLimit.TicketPurchases.Add( new TicketPurchase
            {
                TicketLimit = ticketLimit,
                UserId = userId,
                CreatorId = userId,
                CreationTime = _clock.Now
            } );

            await _concertOneDbContext.SaveChangesAsync( cancellationToken );
        }

        public async Task<IEnumerable<TicketPurchaseListItemDto>> GetPurchasedTicketsAsync(
            Guid userId,
            CancellationToken cancellationToken )
        {
            List<TicketPurchase> ticketPurchases = await _concertOneDbContext.TicketPurchases
                                                            .AsNoTracking()
                                                            .Include( tp => tp.TicketLimit )
                                                                .ThenInclude( tl => tl.TicketCategory )
                                                            .Include( tp => tp.TicketLimit )
                                                                .ThenInclude( tl => tl.Concert )
                                                            .Where( tp => tp.UserId == userId )
                                                            .ToListAsync( cancellationToken );
            return ticketPurchases.Select( tp => new TicketPurchaseListItemDto
            {
                Artist = tp.TicketLimit.Concert.Artist,
                Location = tp.TicketLimit.Concert.Location,
                PurchaseTime = tp.CreationTime,
                StartTime = tp.TicketLimit.Concert.StartTime,
                TicketCategory = tp.TicketLimit.TicketCategory.Name,
                UnitPrice = tp.TicketLimit.UnitPrice
            } );
        }
    }
}
