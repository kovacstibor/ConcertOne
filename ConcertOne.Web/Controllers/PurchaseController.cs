using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Entity;
using ConcertOne.Dal.Identity;
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
        private readonly UserManager<User> _userManager;

        public PurchaseController(
            ITicketService ticketService,
            UserManager<User> userManager )
        {
            _ticketService = ticketService ?? throw new ArgumentNullException( nameof( ticketService ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
        }

        [Authorize( Roles = "NORMAL,PRIVILEDGED" )]
        [HttpGet]
        [Route( "api/v1/" + PurchaseController.Name )]
        public async Task<IActionResult> GetPurchasesAsync()
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();
                IEnumerable<Ticket> purchasedTickets = await _ticketService.GetPurchasedTickets( userId );
                return Json( purchasedTickets.Select( tp => new TicketViewModel( tp ) ).ToList() );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [Authorize( Roles = "NORMAL,PRIVILEDGED" )]
        [HttpPost]
        [Route( "api/v1/" + PurchaseController.Name )]
        public async Task<IActionResult> PurchaseTicketsAsync( [FromBody] PurchaseViewModel purchase )
        {
            try
            {
                Guid userId = await GetCurrentUserIdAsync();
                await _ticketService.PurchaseTicketAsync(
                    userid: userId,
                    concertId: purchase.ConcertId,
                    purchases: purchase.PurchasedTickets );
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

        private async Task<Guid> GetCurrentUserIdAsync()
        {
            User currentUser = await _userManager.GetUserAsync( User );
            if (currentUser == null)
            {
                throw new InvalidOperationException();
            }

            return currentUser.Id;
        }
    }
}
