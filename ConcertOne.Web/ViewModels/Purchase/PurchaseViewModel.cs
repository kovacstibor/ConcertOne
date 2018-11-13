using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace ConcertOne.Web.ViewModels.Purchase
{
    public class PurchaseViewModel
    {
        [JsonProperty( "ConcertId" )]
        public Guid ConcertId { get; set; }

        [JsonProperty( "PurchasedTickets" )]
        public Dictionary<Guid, int> PurchasedTickets { get; set; }

        public PurchaseViewModel()
        {
            PurchasedTickets = new Dictionary<Guid, int>();
        }
    }
}
