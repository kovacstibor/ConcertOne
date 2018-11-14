using Newtonsoft.Json;

using System;

namespace ConcertOne.Web.ViewModels.Purchase
{
    public class PurchaseViewModel
    {
        [JsonProperty( "ConcertId" )]
        public Guid ConcertId { get; set; }

        [JsonProperty( "TicketCategoryId" )]
        public Guid TicketCategoryId { get; set; }
    }
}
