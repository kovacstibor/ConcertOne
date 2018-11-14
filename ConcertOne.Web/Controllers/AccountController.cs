using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Web.ViewModels.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace ConcertOne.Web.Controllers
{
    public class AccountController : Controller
    {
        public const string Name = "Account";

        private readonly IAccountService _accountService;

        public AccountController( IAccountService accountService )
            : base()
        {
            _accountService = accountService ?? throw new ArgumentNullException( nameof( accountService ) );
        }

        [AllowAnonymous]
        [HttpPost]
        [Route( "api/v1/" + AccountController.Name + "/Login" )]
        public async Task<IActionResult> LoginAsync( [FromBody] LoginViewModel login )
        {
            if (login == null || !ModelState.IsValid)
            {
                return StatusCode( 400 );
            }

            try
            {
                await _accountService.LoginAsync( login.EmailAddress, login.Password );
                return StatusCode( User.IsInRole( "PRIVILEDGED" ) ? 201 : 204 );
            }
            catch (BllException)
            {
                return StatusCode( 401 );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [Authorize( Roles = "NORMAL,PRIVILEDGED" )]
        [HttpPost]
        [Route( "api/v1/" + AccountController.Name + "/Logout" )]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await _accountService.LogoutAsync();
                return StatusCode( 204 );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route( "api/v1/" + AccountController.Name + "/Register" )]
        public async Task<IActionResult> RegisterAsync( [FromBody] RegisterViewModel registration )
        {
            if (registration == null || !ModelState.IsValid)
            {
                return StatusCode( 400 );
            }

            try
            {
                await _accountService.RegisterAsync(
                    emailAddress: registration.EmailAddress,
                    password: registration.Password );
                return StatusCode( 201 );
            }
            catch (BllException)
            {
                return StatusCode( 400 );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }
    }
}