using ConcertOne.Dal.Behavior;
using ConcertOne.Dal.Entity;
using ConcertOne.Dal.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Dal.DataContext
{
    public sealed class ConcertOneDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Concert> Concerts { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

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
            ConfigureTicketEntity( builder: builder );
            ConfigureTicketCategoryEntity( builder: builder );

            // Configure identity entities
            ConfigureUserEntity( builder: builder );
            base.OnModelCreating( builder: builder );
        }

        public Task<int> SaveChangesAsync(
            Guid modifierId,
            DateTime currentTime,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            IEnumerable<IAuditable> createdEntities = ChangeTracker.Entries()
                                                                   .Where( e => e.State == EntityState.Added )
                                                                   .Where( e => e.Entity is IAuditable )
                                                                   .Select( e => e.Entity as IAuditable );
            IEnumerable<IAuditable> modifiedEntities = ChangeTracker.Entries()
                                                                    .Where( e => e.State == EntityState.Modified )
                                                                    .Where( e => e.Entity is IAuditable )
                                                                    .Select( e => e.Entity as IAuditable );

            foreach (IAuditable createdEntity in createdEntities)
            {
                createdEntity.CreatorId = modifierId;
                createdEntity.CreationTime = currentTime;
            }

            foreach (IAuditable modifiedEntity in modifiedEntities)
            {
                modifiedEntity.LastModifierId = modifierId;
                modifiedEntity.LastModificationTime = currentTime;
            }

            return SaveChangesAsync( cancellationToken: cancellationToken );
        }

        private void ConfigureUserEntity( ModelBuilder builder )
        {
            builder.Entity<User>().HasMany( u => u.TicketPurchases )
                                  .WithOne( tp => tp.User ).HasForeignKey( tp => tp.UserId );
        }

        private void ConfigureConcertEntity( ModelBuilder builder )
        {
            builder.Entity<Concert>().HasMany( c => c.TicketLimits )
                                     .WithOne( tl => tl.Concert ).HasForeignKey( tl => tl.ConcertId );
            builder.Entity<Concert>().HasMany( c => c.TicketPurchases )
                                     .WithOne( tp => tp.Concert ).HasForeignKey( tp => tp.ConcertId );
        }

        private void ConfigureTicketCategoryEntity( ModelBuilder builder )
        {
            builder.Entity<TicketCategory>().HasMany( c => c.TicketLimits )
                                            .WithOne( tl => tl.TicketCategory ).HasForeignKey( tl => tl.TicketCategoryId );
            builder.Entity<TicketCategory>().HasMany( c => c.Tickets )
                                            .WithOne( t => t.TicketCategory ).HasForeignKey( t => t.TicketCategoryId );
        }

        private void ConfigureTicketEntity( ModelBuilder builder )
        {
            builder.Entity<Ticket>().HasOne( t => t.TicketPurchase )
                                    .WithMany( tp => tp.Tickets ).HasForeignKey( t => t.TicketPurchaseId );
        }
    }
}
