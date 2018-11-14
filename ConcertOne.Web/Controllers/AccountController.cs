using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Identity;
using ConcertOne.Web.Services;
using ConcertOne.Web.ViewModels.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace ConcertOne.Web.Controllers
{
    public class AccountController : Controller
    {
        public const string Name = "Account";

        private readonly IAccountService _accountService;
        private readonly SessionIdService _sessionIdService;
        private readonly UserManager<User> _userManager;

        public AccountController(
            IAccountService accountService,
            SessionIdService sessionIdService,
            UserManager<User> userManager)
            : base()
        {
            _accountService = accountService ?? throw new ArgumentNullException( nameof( accountService ) );
            _sessionIdService = sessionIdService ?? throw new ArgumentNullException( nameof( sessionIdService ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
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
                User user = await _userManager.FindByNameAsync( login.EmailAddress );
                if (user == null)
                {
                    throw new BllException( "User not found!" );
                }

                bool isPriviledgedUser = await _userManager.IsInRoleAsync( user, "PRIVILEDGED" );
                string sessionId;
                object result;
                if (isPriviledgedUser)
                {
                    sessionId = _sessionIdService.CreatePriviledgedSession( user.Id );
                    result = new { SessionId = sessionId, IsAdmin = true };
                }
                else
                {
                    sessionId = _sessionIdService.CreateNormalSession( user.Id );
                    result = new { SessionId = sessionId, IsAdmin = false };
                }

                return StatusCode( 200, result );
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

        [AllowAnonymous]
        [HttpPost]
        [Route( "api/v1/" + AccountController.Name + "/Logout" )]
        public async Task<IActionResult> LogoutAsync(
            [FromHeader( Name = "SessionId" )] string sessionId )
        {
            _sessionIdService.RemoveSessionId( sessionId );
            return StatusCode( 204 );
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