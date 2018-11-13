using ConcertOne.Bll.Dto;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Entity;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.ServiceImplementation
{
    public class TicketCategoryService : ITicketCategoryService
    {
        public Task CreateTicketCategoryAsync( TicketCategoryDataDto ticketCategory, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            throw new NotImplementedException();
        }

        public Task DeleteTicketCategoryAsync( Guid ticketCategoryId, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketCategory>> GetTicketCategoriesAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            throw new NotImplementedException();
        }
    }
}
