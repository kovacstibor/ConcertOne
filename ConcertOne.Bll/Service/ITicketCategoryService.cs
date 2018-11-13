using ConcertOne.Bll.Dto;
using ConcertOne.Dal.Entity;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.Service
{
    public interface ITicketCategoryService
    {
        Task<IEnumerable<TicketCategory>> GetTicketCategoriesAsync( CancellationToken cancellationToken = default( CancellationToken ) );

        Task CreateTicketCategoryAsync(
            TicketCategoryDataDto ticketCategory,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task DeleteTicketCategoryAsync(
            Guid ticketCategoryId,
            CancellationToken cancellationToken = default( CancellationToken ) );
    }
}
