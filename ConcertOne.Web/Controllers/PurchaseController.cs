using ConcertOne.Bll.Dto.TicketPurchase;
using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Entity;
using ConcertOne.Dal.Identity;
using ConcertOne.Web.Services;
using ConcertOne.Web.ViewModels.Purchase;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertOne.Web.Controllers
{
    public class PurchaseController : Controller
    {
        private const string Name = "Purchase";

        private readonly ITicketService _ticketService;
        private readonly SessionIdService _sessionIdService;
        private readonly UserManager<User> _userManager;

        public PurchaseController(
            ITicketService ticketService,
            SessionIdService sessionIdService,
            UserManager<User> userManager )
        {
            _ticketService = ticketService ?? throw new ArgumentNullException( nameof( ticketService ) );
            _sessionIdService = sessionIdService ?? throw new ArgumentNullException( nameof( sessionIdService ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
        }

        [AllowAnonymous]
        [HttpGet]
        [Route( "api/v1/" + PurchaseController.Name )]
        public async Task<IActionResult> GetPurchasesAsync( [FromHeader( Name = "SessionId" )] string sessionId )
        {
            if (!_sessionIdService.IsValidSessionId( sessionId ))
            {
                return StatusCode( 401 );
            }

            try
            {
                var userId = _sessionIdService.GetUserId( sessionId );
                IEnumerable<TicketPurchaseListItemDto> purchasedTickets = await _ticketService.GetPurchasedTicketsAsync( userId );
                return Json( purchasedTickets.Select( tp => new PurchaseListItemViewModel( tp ) ).ToList() );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route( "api/v1/" + PurchaseController.Name )]
        public async Task<IActionResult> PurchaseTicketsAsync(
            [FromBody] PurchaseViewModel purchase,
            [FromHeader( Name = "SessionId" )] string sessionId )
        {
            if (!_sessionIdService.IsValidSessionId( sessionId ))
            {
                return StatusCode( 401 );
            }

            try
            {
                Guid userId = _sessionIdService.GetUserId( sessionId );
                await _ticketService.PurchaseTicketAsync(
                    concertId: purchase.ConcertId,
                    ticketCategoryId: purchase.TicketCategoryId,
                    userid: userId );
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
