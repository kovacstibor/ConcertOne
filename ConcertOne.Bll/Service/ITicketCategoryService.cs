
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.Service
{
    public interface ITicketCategoryService
    {
        Task<IEnumerable<string>> GetTicketCategoriesAsync( CancellationToken cancellationToken = default( CancellationToken ) );

        Task UpdateTicketCategoriesAsync(
            List<string> ticketCategories,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) );
    }
}
