using System;

namespace ConcertOne.Bll.Dto.Concert
{
    public sealed class TicketLimitDto
    {
        public bool IsAvailable { get; set; }

        public int Limit { get; set; }

        public int UnitPrice { get; set; }

        public Guid TicketCategoryId { get; set; }

        public string TicketCategoryName { get; set; }
    }
}
