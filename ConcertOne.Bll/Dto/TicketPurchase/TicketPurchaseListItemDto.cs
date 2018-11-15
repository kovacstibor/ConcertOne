using System;

namespace ConcertOne.Bll.Dto.TicketPurchase
{
    public sealed class TicketPurchaseListItemDto
    {
        public string Artist { get; set; }

        public string Location { get; set; }

        public DateTime PurchaseTime { get; set; }

        public DateTime StartTime { get; set; }

        public int UnitPrice { get; set; }

        public string TicketCategory { get; set; }
    }
}
