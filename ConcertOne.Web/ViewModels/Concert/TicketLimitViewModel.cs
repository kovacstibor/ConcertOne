using ConcertOne.Bll.Dto.Concert;

using Newtonsoft.Json;

using System;

namespace ConcertOne.Web.ViewModels.Concert
{
    public class TicketLimitViewModel
    {
        [JsonProperty( "IsAvailable" )]
        public bool IsAvailable { get; set; }

        [JsonProperty( "Limit" )]
        public int Limit { get; set; }

        [JsonProperty( "UnitPrice" )]
        public int UnitPrice { get; set; }

        [JsonProperty( "TicketCategoryId" )]
        public Guid TicketCategoryId { get; set; }

        [JsonProperty( "TicketCategoryName" )]
        public string TicketCategoryName { get; set; }

        public TicketLimitViewModel()
        {

        }

        public TicketLimitViewModel( TicketLimitDto ticketLimitDto )
            : this()
        {
            if (ticketLimitDto == null)
            {
                throw new ArgumentNullException( nameof( ticketLimitDto ) );
            }

            IsAvailable = ticketLimitDto.IsAvailable;
            Limit = ticketLimitDto.Limit;
            UnitPrice = ticketLimitDto.UnitPrice;
            TicketCategoryId = ticketLimitDto.TicketCategoryId;
            TicketCategoryName = ticketLimitDto.TicketCategoryName;
        }

        public TicketLimitDto ToDto()
        {
            return new TicketLimitDto
            {
                IsAvailable = IsAvailable,
                Limit = Limit,
                UnitPrice = UnitPrice,
                TicketCategoryId = TicketCategoryId,
                TicketCategoryName = TicketCategoryName
            };
        }
    }
}
