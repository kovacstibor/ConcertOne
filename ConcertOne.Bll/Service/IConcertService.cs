using ConcertOne.Bll.Dto;
using ConcertOne.Dal.Entity;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.Service
{
    public interface IConcertService
    {
        Task<IEnumerable<Concert>> GetConcertsAsync( CancellationToken cancellationToken = default( CancellationToken ) );

        Task<Concert> GetConcertDetailsAsync(
            Guid concertId,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task CreateConcertAsync(
            ConcertDataDto concert,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task UpdateConcertAsync(
            Guid concertId,
            ConcertDataDto modifiedConcert,
            Guid userId,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task DeleteConcertAsync(
            Guid concertId,
            CancellationToken cancellationToken = default( CancellationToken ) );
    }
}
