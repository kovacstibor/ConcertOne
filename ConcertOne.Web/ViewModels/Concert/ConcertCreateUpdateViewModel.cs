using ConcertOne.Bll.Dto.Concert;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ConcertOne.Web.ViewModels.Concert
{
    public class ConcertCreateUpdateViewModel
    {
        [JsonProperty( "Artist" )]
        public string Artist { get; set; }

        [JsonProperty( "Description" )]
        public string Description { get; set; }

        [JsonProperty( "Location" )]
        public string Location { get; set; }

        [JsonProperty( "StartDate" )]
        public string StartDate { get; set; }

        [JsonProperty( "StartTime" )]
        public string StartTime { get; set; }

        [JsonProperty( "Tags" )]
        public List<string> Tags { get; set; }

        [JsonProperty( "TicketLimits" )]
        public List<TicketLimitViewModel> TicketLimits { get; set; }

        public ConcertCreateUpdateViewModel()
        {
            TicketLimits = new List<TicketLimitViewModel>();
        }

        public CreateUpdateConcertDto ToDto()
        {
            return new CreateUpdateConcertDto
            {
                Artist = Artist,
                Description = Description,
                Location = Location,
                StartTime = DateTime.ParseExact( $"{StartDate};{StartTime}", "yyyy-MM-dd;HH:mm", CultureInfo.InvariantCulture ),
                Tags = Tags,
                TicketLimits = TicketLimits.Select( tl => tl.ToDto() ).ToList()
            };
        }
    }
}
