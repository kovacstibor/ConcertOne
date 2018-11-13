using ConcertOne.Bll.Service;
using ConcertOne.Bll.ServiceImplementation;
using ConcertOne.Common.Service;
using ConcertOne.Common.ServiceInterface;
using ConcertOne.Dal.DataContext;
using ConcertOne.Dal.Identity;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace ConcertOne.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup( IConfiguration configuration )
        {
            _configuration = configuration ?? throw new ArgumentNullException( nameof( configuration ) );
        }

        public void ConfigureServices( IServiceCollection services )
        {
            // Register cookie policy services to conform the GDPR regulations
            services.Configure<CookiePolicyOptions>( options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            } );

            // Register database context
            services.AddDbContext<ConcertOneDbContext>( options =>
            {
                options.UseSqlServer( _configuration.GetConnectionString( "ConcertOne" ) );
            } );

            // Register identity services
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ConcertOneDbContext>();

            // Register MVC services
            services.AddMvc()
                .SetCompatibilityVersion( CompatibilityVersion.Version_2_1 );
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register .Common services
            services.AddSingleton<IClock, Clock>();

            // Register .Dal services
            services.AddScoped<ConcertOneDbContextInitializer>();

            // Register .Bll services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IConcertService, ConcertService>();
            services.AddScoped<ITicketCategoryService, TicketCategoryService>();
            services.AddScoped<ITicketService, TicketService>();
        }

        public void Configure(
            IApplicationBuilder applicationPipeline,
            IHostingEnvironment executionEnvironment,
            ConcertOneDbContextInitializer concertOneDbContextInitializer )
        {
            if (executionEnvironment.IsDevelopment())
            {
                applicationPipeline.UseDeveloperExceptionPage();
                applicationPipeline.UseDatabaseErrorPage();
            }
            else
            {
                applicationPipeline.UseExceptionHandler( "/Error" );
                applicationPipeline.UseHsts();
            }

            applicationPipeline.UseHttpsRedirection();
            applicationPipeline.UseStaticFiles();
            applicationPipeline.UseCookiePolicy();

            applicationPipeline.UseAuthentication();

            applicationPipeline.UseMvc();

            // Initialize the content of the database if necessary
            concertOneDbContextInitializer.InitializeDatabaseAsync().GetAwaiter().GetResult();
        }
    }
}
