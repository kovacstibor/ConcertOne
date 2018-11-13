using ConcertOne.Dal.Behavior;

using Microsoft.AspNetCore.Identity;

using System;

namespace ConcertOne.Dal.Identity
{
    public sealed class Role : IdentityRole<Guid>, IConcertOneEntity
    {

    }
}
