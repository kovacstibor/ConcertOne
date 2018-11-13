using ConcertOne.Bll.Service;
using ConcertOne.Dal.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.ServiceImplementation
{
    public class TicketService : ITicketService
    {
        public Task<IEnumerable<Ticket>> GetPurchasedTickets( Guid userId, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            throw new NotImplementedException();
        }

        public Task PurchaseTicketAsync( Guid userid, Guid concertId, Guid ticketCategoryId, int numberOfTickets, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            throw new NotImplementedException();
        }
    }
}
