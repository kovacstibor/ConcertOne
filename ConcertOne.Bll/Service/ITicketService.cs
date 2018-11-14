using ConcertOne.Bll.Dto.TicketPurchase;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.Service
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketPurchaseListItemDto>> GetPurchasedTicketsAsync(
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task PurchaseTicketAsync(
            Guid concertId,
            Guid ticketCategoryId,
            Guid userid,
            CancellationToken cancellationToken = default( CancellationToken ) );
    }
}
