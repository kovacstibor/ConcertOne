using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertOne.Web.Controllers
{
    public class TicketCategoryController : Controller
    {
        public const string Name = "TicketCategory";

        private readonly ITicketCategoryService _ticketCategoryService;
        private readonly UserManager<User> _userManager;

        public TicketCategoryController(
            ITicketCategoryService ticketCategoryService,
            UserManager<User> userManager )
        {
            _ticketCategoryService = ticketCategoryService ?? throw new ArgumentNullException( nameof( ticketCategoryService ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
        }

        [Authorize( Roles = "PRIVILEDGED" )]
        [HttpGet]
        [Route( "api/v1/" + TicketCategoryController.Name )]
        public async Task<IActionResult> GetTicketCategoriesAsync()
        {
            try
            {
                IEnumerable<string> ticketCategories = await _ticketCategoryService.GetTicketCategoriesAsync();
                return Json( ticketCategories.ToList() );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [Authorize( Roles = "PRIVILEDGED" )]
        [HttpPut]
        [Route( "api/v1/" + TicketCategoryController.Name )]
        public async Task<IActionResult> UpdateTicketCategoryAsync( [FromBody] List<string> ticketCategories )
        {
            try
            {
                Guid userId = await GetCurrentUserIdAsync();
                await _ticketCategoryService.UpdateTicketCategoriesAsync(
                    ticketCategories: ticketCategories,
                    userId: userId );
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
