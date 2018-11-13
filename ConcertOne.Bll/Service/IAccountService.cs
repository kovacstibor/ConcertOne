using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.Service
{
    public interface IAccountService
    {
        Task LoginAsync(
            string emailAddress,
            string password,
            CancellationToken cancellationToken = default( CancellationToken ) );

        Task LogoutAsync( CancellationToken cancellationToken = default( CancellationToken ) );

        Task RegisterAsync(
            string emailAddress,
            string password,
            CancellationToken cancellationToken = default( CancellationToken ) );
    }
}
