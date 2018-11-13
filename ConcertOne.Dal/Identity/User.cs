using ConcertOne.Dal.Behavior;
using ConcertOne.Dal.Entity;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;

namespace ConcertOne.Dal.Identity
{
    public sealed class User : IdentityUser<Guid>, IConcertOneEntity
    {
        public ICollection<TicketPurchase> TicketPurchases { get; set; }

        public User()
        {
            TicketPurchases = new HashSet<TicketPurchase>();
        }
    }
}
