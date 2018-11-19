using ConcertOne.Bll.Dto.TicketCategory;
using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Identity;
using ConcertOne.Web.Services;
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
        private readonly SessionIdService _sessionIdService;
        private readonly UserManager<User> _userManager;

        public TicketCategoryController(
            ITicketCategoryService ticketCategoryService,
            SessionIdService sessionIdService,
            UserManager<User> userManager )
        {
            _ticketCategoryService = ticketCategoryService ?? throw new ArgumentNullException( nameof( ticketCategoryService ) );
            _sessionIdService = sessionIdService ?? throw new ArgumentNullException( nameof( sessionIdService ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
        }

        [AllowAnonymous]
        [HttpGet]
        [Route( "api/v1/" + TicketCategoryController.Name )]
        public async Task<IActionResult> GetTicketCategoriesAsync( [FromHeader( Name = "SessionId" )] string sessionId )
        {
            if (!_sessionIdService.IsValidSessionId( sessionId ))
            {
                return StatusCode( 401 );
            }

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

        [AllowAnonymous]
        [HttpGet]
        [Route( "api/v1/" + TicketCategoryController.Name + "/List" )]
        public async Task<IActionResult> GetTicketCategoriesWithIdsAsync( [FromHeader( Name = "SessionId" )] string sessionId )
        {
            if (!_sessionIdService.IsValidSessionId( sessionId ))
            {
                return StatusCode( 401 );
            }

            try
            {
                IEnumerable<TicketCategoryListItemDto> ticketCategories = await _ticketCategoryService.GetTicketCategoriesWithIdsAsync();
                return Json( ticketCategories.Select( tc => new TicketCategoryListItemViewModel( tc ) ).ToList() );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route( "api/v1/" + TicketCategoryController.Name )]
        public async Task<IActionResult> UpdateTicketCategoryAsync(
            [FromBody] List<string> ticketCategories,
            [FromHeader( Name = "SessionId" )] string sessionId )
        {
            if (!_sessionIdService.IsPriviledged( sessionId ))
            {
                return StatusCode( 401 );
            }

            try
            {
                Guid userId = _sessionIdService.GetUserId( sessionId );
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
    }
}
