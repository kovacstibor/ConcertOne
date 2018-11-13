using ConcertOne.Dal.Entity;
using Newtonsoft.Json;
using System;

namespace ConcertOne.Web.ViewModels.Concert
{
    public class TicketLimitViewModel
    {
        [JsonProperty( "Limit" )]
        public int Limit { get; set; }

        [JsonProperty( "TicketCategoryName" )]
        public string TicketCategoryName { get; set; }

        [JsonProperty( "Monetary" )]
        public string Monetary { get; set; }

        [JsonProperty( "UnitPrice" )]
        public double UnitPrice { get; set; }

        public TicketLimitViewModel()
        {

        }

        public TicketLimitViewModel( TicketLimit ticketLimit )
        {
            if (ticketLimit == null)
            {
                throw new ArgumentNullException( nameof( ticketLimit ) );
            }

            if (ticketLimit.TicketCategory == null)
            {
                throw new ArgumentNullException( nameof( ticketLimit.TicketCategory ) );
            }

            Limit = ticketLimit.Limit;
            Monetary = ticketLimit.TicketCategory.Monetary;
            TicketCategoryName = ticketLimit.TicketCategory.Name;
            UnitPrice = ticketLimit.TicketCategory.UnitPrice;
        }
    }
}
