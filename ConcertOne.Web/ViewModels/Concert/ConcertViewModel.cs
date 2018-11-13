using Newtonsoft.Json;

using System;

namespace ConcertOne.Web.ViewModels.Concert
{
    public class ConcertViewModel
    {
        [JsonProperty( "Id" )]
        public Guid Id { get; set; }

        [JsonProperty( "Artist" )]
        public string Artist { get; set; }

        [JsonProperty( "Location" )]
        public string Location { get; set; }

        [JsonProperty( "StartTime" )]
        public DateTime StartTime { get; set; }

        [JsonProperty( "AttachedImageUrl" )]
        public string AttachedImageUrl { get; set; }

        public ConcertViewModel()
        {

        }

        public ConcertViewModel( Dal.Entity.Concert concert )
            : this()
        {
            if (concert == null)
            {
                throw new ArgumentNullException( nameof( concert ) );
            }

            Id = concert.Id;
            Artist = concert.Artist;
            Location = concert.Location;
            StartTime = concert.StartTime;
            AttachedImageUrl = concert.AttachedImageUrl;
        }
    }
}
