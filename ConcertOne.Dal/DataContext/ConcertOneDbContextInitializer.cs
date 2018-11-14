using ConcertOne.Common.ServiceInterface;
using ConcertOne.Dal.Entity;
using ConcertOne.Dal.Identity;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcertOne.Dal.DataContext
{
    public sealed class ConcertOneDbContextInitializer
    {
        private readonly IClock _clock;
        private readonly ConcertOneDbContext _concertOneDbContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public ConcertOneDbContextInitializer(
            IClock clock,
            ConcertOneDbContext concertOneDbContext,
            RoleManager<Role> roleManager,
            UserManager<User> userManager )
        {
            _clock = clock ?? throw new ArgumentNullException( nameof( clock ) );
            _concertOneDbContext = concertOneDbContext ?? throw new ArgumentNullException( nameof( concertOneDbContext ) );
            _roleManager = roleManager ?? throw new ArgumentNullException( nameof( roleManager ) );
            _userManager = userManager ?? throw new ArgumentNullException( nameof( userManager ) );
        }

        public async Task InitializeDatabaseAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            if (_concertOneDbContext.Users.Any())
            {
                return;
            }

            IEnumerable<Role> concertOneRoles = await InitializeRolesAsync( cancellationToken: cancellationToken );
            IEnumerable<User> concertOneUsers = await InitializeUsersAsync(
                concertOneRoles: concertOneRoles,
                cancellationToken: cancellationToken );
            await InitializeConcertsAsync(
                priviledgedUser: concertOneUsers.ElementAt( 1 ),
                cancellationToken: cancellationToken );
        }

        private async Task<IEnumerable<Role>> InitializeRolesAsync( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            IEnumerable<Role> concertOneRoles = new Role[]
            {
                new Role
                {
                    Name = "NORMAL",
                    NormalizedName = "NORMAL"
                },
                new Role
                {
                    Name = "PRIVILEDGED",
                    NormalizedName = "PRIVILEDGED"
                },
            };

            foreach (Role concertOneRole in concertOneRoles)
            {
                if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                await _roleManager.CreateAsync( concertOneRole );
            }

            return concertOneRoles;
        }

        private async Task<IEnumerable<User>> InitializeUsersAsync(
            IEnumerable<Role> concertOneRoles,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            IEnumerable<User> concertOneUsers = new User[]
            {
                new User
                {
                    Email = "normaluser@concertone.hu",
                    EmailConfirmed = true,
                    NormalizedEmail = "normaluser@concertone.hu",
                    NormalizedUserName = "normaluser@concertone.hu",
                    PhoneNumber = "+36301234567",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    UserName = "normaluser@concertone.hu"
                },
                new User
                {
                    Email = "priviledgeduser@concertone.hu",
                    EmailConfirmed = true,
                    NormalizedEmail = "priviledgeduser@concertone.hu",
                    NormalizedUserName = "priviledgeduser@concertone.hu",
                    PhoneNumber = "+36307654321",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    UserName = "priviledgeduser@concertone.hu"
                }

            };

            IEnumerable<string> passwords = new string[]
            {
                "^%0HPYRad6wS%F&6-sC082!W",
                "+I@mz!ukO5TyV3EbWIB=XPus"
            };

            for (int i = 0; i < concertOneUsers.Count(); ++i)
            {
                if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                await _userManager.CreateAsync( concertOneUsers.ElementAt( i ), passwords.ElementAt( i ) );
                await _userManager.AddToRoleAsync( concertOneUsers.ElementAt( i ), concertOneRoles.ElementAt( i ).Name );
            }

            return concertOneUsers;
        }

        private async Task InitializeConcertsAsync(
            User priviledgedUser,
            CancellationToken cancellationToken = default( CancellationToken ) )
        {
            Concert metallicaConcert = new Concert
            {
                Artist = "Metallica",
                CreationTime = _clock.Now,
                CreatorId = priviledgedUser.Id,
                Description = "Metallica concert description ...",
                Location = "Papp László SportAréna",
                StartTime = new DateTime( 2018, 5, 5 )
            };
            metallicaConcert.ConcertTags.Add( new ConcertTag
            {
                Name = "trash",
                Concert = metallicaConcert,
                CreatorId = priviledgedUser.Id,
                CreationTime = _clock.Now
            } );
            metallicaConcert.ConcertTags.Add( new ConcertTag
            {
                Name = "metal",
                Concert = metallicaConcert,
                CreatorId = priviledgedUser.Id,
                CreationTime = _clock.Now
            } );

            Concert edSheeranConcert = new Concert
            {
                Artist = "Ed Sheeran",
                CreationTime = _clock.Now,
                CreatorId = priviledgedUser.Id,
                Description = "Ed Sheeran concert description ...",
                Location = "Sziget Fesztivál",
                StartTime = new DateTime( 2019, 8, 7 )
            };
            edSheeranConcert.ConcertTags.Add( new ConcertTag
            {
                Name = "sziget",
                Concert = metallicaConcert,
                CreatorId = priviledgedUser.Id,
                CreationTime = _clock.Now
            } );
            edSheeranConcert.ConcertTags.Add( new ConcertTag
            {
                Name = "ginger",
                Concert = metallicaConcert,
                CreatorId = priviledgedUser.Id,
                CreationTime = _clock.Now
            } );

            _concertOneDbContext.Concerts.Add( metallicaConcert );
            _concertOneDbContext.Concerts.Add( edSheeranConcert );

            await _concertOneDbContext.SaveChangesAsync( cancellationToken );
        }
    }
}
