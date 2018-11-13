using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcertOne.Web.ViewModels.Concert
{
    public class ConcertDetailsViewModel
    {
        [JsonProperty( "Id" )]
        public Guid Id { get; set; }

        [JsonProperty( "Artist" )]
        public string Artist { get; set; }

        [JsonProperty( "Description" )]
        public string Description { get; set; }

        [JsonProperty( "Location" )]
        public string Location { get; set; }

        [JsonProperty( "StartTime" )]
        public DateTime StartTime { get; set; }

        [JsonProperty( "AttachedImageUrl" )]
        public string AttachedImageUrl { get; set; }

        [JsonProperty( "TicketLimits" )]
        public List<TicketLimitViewModel> TicketLimits { get; set; }

        public ConcertDetailsViewModel()
        {
            TicketLimits = new List<TicketLimitViewModel>();
        }

        public ConcertDetailsViewModel( Dal.Entity.Concert concert )
            : this()
        {
            if (concert == null)
            {
                throw new ArgumentNullException( nameof( concert ) );
            }
            Id = concert.Id;
            Artist = concert.Artist;
            Description = concert.Description;
            Location = concert.Location;
            StartTime = concert.StartTime;
            AttachedImageUrl = concert.AttachedImageUrl;

            if (concert.TicketLimits != null)
            {
                TicketLimits = concert.TicketLimits.Select( tl => new TicketLimitViewModel( tl ) ).ToList();
            }
        }
    }
}
