using Spendings.Data.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Onion.Spendings.Api.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        public ServiceProvider serviceProvider { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's database  registration.
                var serviceDescriptor = services
                                            .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (serviceDescriptor != null)
                {
                    services.Remove(serviceDescriptor);
                }

                // Add MyDbContext using an in-memory database for testing.
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDbConnectionString");
                    
                });
                var servicesProvider = services.BuildServiceProvider();
                serviceProvider = servicesProvider;
                // Create a scope to obtain a reference to the database context (MyDbContext).
                using (var serviceScope = servicesProvider.CreateScope())
                {
                    var scopedServices = serviceScope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                    db.Database.EnsureCreated();
                }
               
            });
        }

        
    }
}
