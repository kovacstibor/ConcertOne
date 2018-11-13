using ConcertOne.Dal.Entity;

using System;

namespace ConcertOne.Web.ViewModels.Purchase
{
    public class TicketViewModel
    {
        public Guid Id { get; set; }

        public DateTime PurchaseTime { get; set; }

        public string CategoryName { get; set; }

        public double Price { get; set; }

        public string Currency { get; set; }

        public string Artist { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public TicketViewModel()
        {

        }

        public TicketViewModel( Ticket ticket )
            : this()
        {
            if (ticket == null)
            {
                throw new ArgumentNullException( nameof( ticket ) );
            }

            Id = ticket.Id;
            PurchaseTime = ticket.CreationTime;

            if (ticket.TicketCategory != null)
            {
                CategoryName = ticket.TicketCategory.Name;
                Price = ticket.TicketCategory.UnitPrice;
                Currency = ticket.TicketCategory.Monetary;
            }

            if (ticket.TicketPurchase != null && ticket.TicketPurchase.Concert != null)
            {
                Artist = ticket.TicketPurchase.Concert.Artist;
                Location = ticket.TicketPurchase.Concert.Location;
                StartTime = ticket.TicketPurchase.Concert.StartTime;
            }
        }
    }
}
