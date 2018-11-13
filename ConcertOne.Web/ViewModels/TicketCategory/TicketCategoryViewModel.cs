using ConcertOne.Bll.Dto;
using Newtonsoft.Json;

using System;

namespace ConcertOne.Web.ViewModels.TicketCategory
{
    public class TicketCategoryViewModel
    {
        [JsonProperty( "Id" )]
        public Guid Id { get; set; }

        [JsonProperty( "Name" )]
        public string Name { get; set; }

        [JsonProperty( "UnitPrice" )]
        public double UnitPrice { get; set; }

        [JsonProperty( "Currency" )]
        public string Currency { get; set; }

        public TicketCategoryViewModel()
        {

        }

        public TicketCategoryViewModel( Dal.Entity.TicketCategory ticketCategory )
            : this()
        {
            if (ticketCategory == null)
            {
                throw new ArgumentNullException( nameof( ticketCategory ) );
            }

            Id = ticketCategory.Id;
            Name = ticketCategory.Name;
            UnitPrice = ticketCategory.UnitPrice;
            Currency = ticketCategory.Monetary;
        }

        public TicketCategoryDataDto ToDto()
        {
            return new TicketCategoryDataDto
            {
                Name = Name,
                UnitPrice = UnitPrice,
                Monetary = Currency
            };
        }
    }
}
