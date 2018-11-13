using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Entity;
using ConcertOne.Dal.Identity;
using ConcertOne.Web.ViewModels.TicketCategory;

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
                IEnumerable<TicketCategory> ticketCategories = await _ticketCategoryService.GetTicketCategoriesAsync();
                return Json( ticketCategories.Select( tc => new TicketCategoryViewModel( tc ) ).ToList() );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [Authorize( Roles = "PRIVILEDGED" )]
        [HttpPost]
        [Route( "api/v1/" + TicketCategoryController.Name )]
        public async Task<IActionResult> CreateTicketCategoryAsync( [FromBody] TicketCategoryViewModel ticketCategory )
        {
            try
            {
                Guid userId = await GetCurrentUserIdAsync();
                await _ticketCategoryService.CreateTicketCategoryAsync(
                    ticketCategory: ticketCategory.ToDto(),
                    userId: userId );
                return StatusCode( 201 );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [Authorize( Roles = "PRIVILEDGED" )]
        [HttpDelete]
        [Route( "api/v1/" + TicketCategoryController.Name + "/{id}" )]
        public async Task<IActionResult> DeleteTicketCategoryAsync( Guid id )
        {
            try
            {
                await _ticketCategoryService.DeleteTicketCategoryAsync( id );
                return StatusCode( 204 );
            }
            catch (BllException)
            {
                return StatusCode( 404 );
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
