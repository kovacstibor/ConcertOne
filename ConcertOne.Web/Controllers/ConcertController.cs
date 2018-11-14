using ConcertOne.Bll.Dto.Concert;
using ConcertOne.Bll.Exception;
using ConcertOne.Bll.Service;
using ConcertOne.Dal.Identity;
using ConcertOne.Web.Services;
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
        private readonly SessionIdService _sessionIdService;
        private readonly UserManager<User> _userManager;

        public ConcertController(
            IConcertService concertService,
            SessionIdService sessionIdService,
            UserManager<User> userManager )
        {
            _concertService = concertService ?? throw new ArgumentNullException( nameof( concertService ) );
            _sessionIdService = sessionIdService ?? throw new ArgumentNullException( nameof( sessionIdService ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
        }

        [AllowAnonymous]
        [HttpGet]
        [Route( "api/v1/" + ConcertController.Name )]
        public async Task<IActionResult> GetConcertsAsync()
        {
            try
            {
                IEnumerable<ConcertListItemDto> concerts = await _concertService.GetConcertsAsync();
                return Json( concerts.Select( c => new ConcertListItemViewModel( c ) ).ToList() );
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
                ConcertDetailsDto concertDetails = await _concertService.GetConcertDetailsAsync( id );
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

        [AllowAnonymous]
        [HttpPost]
        [Route( "api/v1/" + ConcertController.Name )]
        public async Task<IActionResult> CreateConcertAsync(
            [FromHeader( Name = "SessionId" )] string sessionId,
            [FromBody] ConcertCreateUpdateViewModel concert )
        {
            if (!_sessionIdService.IsPriviledged( sessionId ))
            {
                return StatusCode( 401 );
            }

            try
            {
                Guid userId = _sessionIdService.GetUserId( sessionId );
                await _concertService.CreateConcertAsync(
                    concert: concert.ToDto(),
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

        [AllowAnonymous]
        [HttpPut]
        [Route( "api/v1/" + ConcertController.Name + "/{id}" )]
        public async Task<IActionResult> UpdateConcertAsync(
            Guid id,
            [FromHeader( Name = "SessionId" )] string sessionId,
            [FromBody] ConcertCreateUpdateViewModel concert )
        {
            if (!_sessionIdService.IsPriviledged( sessionId ))
            {
                return StatusCode( 401 );
            }

            try
            {
                Guid userId = _sessionIdService.GetUserId( sessionId );
                await _concertService.UpdateConcertAsync(
                    concertId: id,
                    concert: concert.ToDto(),
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

        [AllowAnonymous]
        [HttpDelete]
        [Route( "api/v1/" + ConcertController.Name + "/{id}" )]
        public async Task<IActionResult> DeleteConcertAsync(
            Guid id,
            [FromHeader( Name = "SessionId" )] string sessionId )
        {
            if (!_sessionIdService.IsPriviledged( sessionId ))
            {
                return StatusCode( 401 );
            }

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
    }
}
