using ConcertOne.Bll.Dto;
using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Entity;
using ConcertOne.Dal.Identity;
using ConcertOne.Web.ViewModels.Concert;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertOne.Web.Controllers
{
    public class ConcertController : Controller
    {
        public const string Name = "Concert";

        private readonly IConcertService _concertService;
        private readonly UserManager<User> _userManager;

        public ConcertController(
            IConcertService concertService,
            UserManager<User> userManager )
        {
            _concertService = concertService ?? throw new ArgumentNullException( nameof( concertService ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
        }

        [AllowAnonymous]
        [HttpGet]
        [Route( "api/v1/" + ConcertController.Name )]
        public async Task<IActionResult> GetConcertsAsync()
        {
            try
            {
                IEnumerable<Concert> concerts = await _concertService.GetConcertsAsync();
                return Json( concerts.Select( c => new ConcertViewModel( c ) ).ToList() );
            }
            catch
            {
                return StatusCode( 500 );
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route( "api/v1/" + ConcertController.Name + "/{id}" )]
        public async Task<IActionResult> GetConcertDetailsAsync( Guid id )
        {
            try
            {
                Concert concertDetails = await _concertService.GetConcertDetailsAsync( id );
                return Json( new ConcertDetailsViewModel( concertDetails ) );
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

        [Authorize( Roles = "PRIVILEDGED" )]
        [HttpPost]
        [Route( "api/v1/" + ConcertController.Name )]
        public async Task<IActionResult> CreateConcertAsync( [FromBody] ConcertDataDto concert )
        {
            try
            {
                Guid userId = await GetCurrentUserId();
                await _concertService.CreateConcertAsync(
                    concert: concert,
                    userId: userId );
                return StatusCode( 201 );
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

        [Authorize( Roles = "PRIVILEDGED" )]
        [HttpPut]
        [Route( "api/v1/" + ConcertController.Name + "/{id}" )]
        public async Task<IActionResult> UpdateConcertAsync(
            Guid id,
            [FromBody] ConcertDataDto concert )
        {
            try
            {
                Guid userId = await GetCurrentUserId();
                await _concertService.UpdateConcertAsync(
                    concertId: id,
                    modifiedConcert: concert,
                    userId: userId );
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

        [Authorize( Roles = "PRIVILEDGED" )]
        [HttpDelete]
        [Route( "api/v1/" + ConcertController.Name + "/{id}" )]
        public async Task<IActionResult> DeleteConcertAsync( Guid id )
        {
            try
            {
                await _concertService.DeleteConcertAsync( id );
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

        private async Task<Guid> GetCurrentUserId()
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
