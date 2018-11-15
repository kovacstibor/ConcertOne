using ConcertOne.Bll.Dto.TicketCategory;

using Newtonsoft.Json;

using System;

namespace ConcertOne.Web.ViewModels.TicketCategory
{
    public class TicketCategoryListItemViewModel
    {
        [JsonProperty( "Id" )]
        public Guid Id { get; set; }

        [JsonProperty( "Name" )]
        public string Name { get; set; }

        public TicketCategoryListItemViewModel()
        {

        }

        public TicketCategoryListItemViewModel( TicketCategoryListItemDto ticketCategoryListItemDto )
            : this()
        {
            if (ticketCategoryListItemDto == null)
            {
                throw new ArgumentNullException( nameof( ticketCategoryListItemDto ) );
            }

            Id = ticketCategoryListItemDto.Id;
            Name = ticketCategoryListItemDto.Name;
        }
    }
}
