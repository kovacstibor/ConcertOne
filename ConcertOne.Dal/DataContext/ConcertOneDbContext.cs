using ConcertOne.Dal.Entity;
using ConcertOne.Dal.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;

namespace ConcertOne.Dal.DataContext
{
    public sealed class ConcertOneDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Concert> Concerts { get; set; }

        public DbSet<ConcertTag> ConcertTags { get; set; }

        public DbSet<TicketCategory> TicketCategories { get; set; }

        public DbSet<TicketLimit> TicketLimits { get; set; }

        public DbSet<TicketPurchase> TicketPurchases { get; set; }

        public ConcertOneDbContext( DbContextOptions<ConcertOneDbContext> options )
            : base( options: options )
        {

        }

        protected override void OnModelCreating( ModelBuilder builder )
        {
            // Configure business entities
            ConfigureConcertEntity( builder: builder );
            ConfigureTicketCategoryEntity( builder: builder );
            ConfigureTicketPurchaseEntity( builder: builder );

            // Configure identity entities
            ConfigureUserEntity( builder: builder );
            base.OnModelCreating( builder: builder );
        }

        private void ConfigureUserEntity( ModelBuilder builder )
        {
            builder.Entity<User>().HasMany( u => u.TicketPurchases )
                                  .WithOne( tp => tp.User ).HasForeignKey( tp => tp.UserId );
        }

        private void ConfigureConcertEntity( ModelBuilder builder )
        {
            builder.Entity<Concert>().HasMany( c => c.ConcertTags )
                                     .WithOne( ct => ct.Concert ).HasForeignKey( ct => ct.ConcertId );

            builder.Entity<Concert>().HasMany( c => c.TicketLimits )
                                     .WithOne( tl => tl.Concert ).HasForeignKey( tl => tl.ConcertId );
        }

        private void ConfigureTicketCategoryEntity( ModelBuilder builder )
        {
            builder.Entity<TicketCategory>().HasMany( c => c.TicketLimits )
                                            .WithOne( tl => tl.TicketCategory ).HasForeignKey( tl => tl.TicketCategoryId );
        }

        private void ConfigureTicketPurchaseEntity( ModelBuilder builder )
        {
            builder.Entity<TicketPurchase>().HasOne( tp => tp.TicketLimit )
                                            .WithMany( tl => tl.TicketPurchases ).HasForeignKey( tp => tp.TicketLimitId );
        }
    }
}
