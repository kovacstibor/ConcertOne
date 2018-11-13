using ConcertOne.Dal.Entity;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.Service
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetPurchasedTickets(
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task PurchaseTicketAsync(
            Guid userid,
            Guid concertId,
            Dictionary<Guid, int> purchases,
            CancellationToken cancellationToken = default( CancellationToken ) );
    }
}
