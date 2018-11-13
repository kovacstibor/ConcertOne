using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Identity;

using Microsoft.AspNetCore.Identity;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Bll.ServiceImplementation
{
    public sealed class AccountService : IAccountService
    {
        private const string NormalRoleName = "NORMAL";

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountService(
            SignInManager<User> signInManager,
            UserManager<User> userManager )
        {
            _signInManager = signInManager ?? throw new ArgumentNullException( nameof( signInManager ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
        }

        public async Task LoginAsync(
            string emailAddress,
            string password,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            User user = await _userManager.FindByNameAsync( userName: emailAddress );
            if (user == null)
            {
                throw new BllException( $"Invalid username or password!" );
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(
                                            user: user,
                                            password: password );
            if (!isValidPassword)
            {
                throw new BllException( $"Invalid username or password!" );
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(
                                                userName: emailAddress,
                                                password: password,
                                                isPersistent: false,
                                                lockoutOnFailure: false );
            if (!signInResult.Succeeded)
            {
                throw new BllException( $"User is locked out of the system!" );
            }
        }

        public Task LogoutAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            return _signInManager.SignOutAsync();
        }

        public async Task RegisterAsync(
            string emailAddress,
            string password,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            User newUser = new User
            {
                Email = emailAddress,
                EmailConfirmed = true,
                NormalizedEmail = emailAddress,
                NormalizedUserName = emailAddress,
                PhoneNumber = string.Empty,
                PhoneNumberConfirmed = true,
                LockoutEnabled = true,
                UserName = emailAddress
            };

            IdentityResult registrationResult = await _userManager.CreateAsync(
                user: newUser,
                password: password );

            if (registrationResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(
                    user: newUser,
                    role: NormalRoleName );
            }
            else
            {
                string registrationErrors = string.Concat( ';', registrationResult.Errors.Select( error => $"'{error.Description}'" ) );
                throw new BllException( $"Error during registration: {registrationErrors}" );
            }
        }
    }
}
