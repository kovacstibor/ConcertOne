using ConcertOne.Bll.Dto;
using ConcertOne.Bll.Dto.Concert;
using ConcertOne.Dal.Entity;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.Service
{
    public interface IConcertService
    {
        Task<IEnumerable<ConcertListItemDto>> GetConcertsAsync( CancellationToken cancellationToken = default( CancellationToken ) );

        Task<ConcertDetailsDto> GetConcertDetailsAsync(
            Guid concertId,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task CreateConcertAsync(
            CreateUpdateConcertDto concert,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task UpdateConcertAsync(
            Guid concertId,
            CreateUpdateConcertDto concert,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task DeleteConcertAsync(
            Guid concertId,
            CancellationToken cancellationToken = default( CancellationToken ) );
    }
}
