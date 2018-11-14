using ConcertOne.Bll.Dto.TicketPurchase;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace ConcertOne.Web.ViewModels.Purchase
{
    public class PurchaseListItemViewModel
    {
        [JsonProperty( "Artist" )]
        public string Artist { get; set; }

        [JsonProperty( "Location" )]
        public string Location { get; set; }

        [JsonProperty( "PurchaseTime" )]
        public DateTime PurchaseTime { get; set; }

        [JsonProperty( "StartTime" )]
        public DateTime StartTime { get; set; }

        [JsonProperty( "UnitPrice" )]
        public int UnitPrice { get; set; }

        [JsonProperty( "TicketCategory" )]
        public string TicketCategory { get; set; }

        public PurchaseListItemViewModel()
        {

        }

        public PurchaseListItemViewModel( TicketPurchaseListItemDto purchaseListItemDto )
            : this()
        {
            if (purchaseListItemDto == null)
            {
                throw new ArgumentNullException( nameof( purchaseListItemDto ) );
            }

            Artist = purchaseListItemDto.Artist;
            Location = purchaseListItemDto.Location;
            PurchaseTime = purchaseListItemDto.PurchaseTime;
            StartTime = purchaseListItemDto.StartTime;
            TicketCategory = purchaseListItemDto.TicketCategory;
            UnitPrice = purchaseListItemDto.UnitPrice;
        }
    }
}
