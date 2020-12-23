using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTestsIssue.Tests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<BloggingContext>));

                services.Remove(descriptor);

                services.AddDbContext<BloggingContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<BloggingContext>();

                db.Database.EnsureCreated();

                db.Blogs.Add(new Blog
                {
                    Url = "/my-first-blog"
                });
                db.SaveChanges();
            });
        }
    }

    public class BasicTests
        : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public BasicTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/blogging");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var model = await response.Content.ReadFromJsonAsync<Blog[]>();
            Assert.NotNull(model);
            Assert.NotEmpty(model);
            Assert.Equal("/my-first-blog", model![0].Url);
        }
    }
}
