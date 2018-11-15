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

        [JsonProperty( "PurchaseDate" )]
        public string PurchaseDate { get; set; }

        [JsonProperty( "PurchaseTime" )]
        public string PurchaseTime { get; set; }

        [JsonProperty( "StartDate" )]
        public string StartDate { get; set; }

        [JsonProperty( "StartTime" )]
        public string StartTime { get; set; }

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
            PurchaseDate = purchaseListItemDto.PurchaseTime.ToString( "yyyy-MM-dd" );
            PurchaseTime = purchaseListItemDto.PurchaseTime.ToString( "HH:mm" );
            StartDate = purchaseListItemDto.StartTime.ToString( "yyyy-MM-dd" );
            StartTime = purchaseListItemDto.StartTime.ToString( "HH:mm" );
            TicketCategory = purchaseListItemDto.TicketCategory;
            UnitPrice = purchaseListItemDto.UnitPrice;
        }
    }
}
