using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Newtonsoft.Json;
using System.Text;
using Spendings.Data.DB;
using Microsoft.Extensions.DependencyInjection;

namespace Onion.Spendings.Api.IntegrationTests.Category
{
   public class CategoryControllerMethodsWorkTest :
    IClassFixture<CustomWebApplicationFactory<onion_spendings.Startup>>
    {

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<onion_spendings.Startup>
            _factory;
        public CategoryControllerMethodsWorkTest(
            CustomWebApplicationFactory<onion_spendings.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task CategoryPostAsync_IfMethodWorks_ReturnOk()
        {
            // Arrange
            global::Spendings.Orchrestrators.Categories.Contracts.InCategory category = new global::Spendings.Orchrestrators.Categories.Contracts.InCategory
            {
                Name = "hobbys"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"/Spendings/v1/Categories")
            {
                Content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
            };
            //Act
            var responce = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
        }

        [Fact]
        public async Task CategoryGetAsync_IfReturnsCorrectModel_ReturnOk()
        {
            // Arrange

            global::Spendings.Data.Categories.Category addedCategory = new global::Spendings.Data.Categories.Category
            {
                Name = "games"
            };
            global::Spendings.Data.Categories.Category categoryFromDB;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                categoryFromDB = DB.Categories.Add(addedCategory).Entity;
                DB.SaveChanges();
            }

            //Act
            var getResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/Spendings/v1/Categories/{categoryFromDB.Id}"));

            var returnedCategory = await ConvertHttpResponceToObj(getResponse);

            // Assert
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(returnedCategory.Name, returnedCategory.Name);
        }

        [Fact]
        public async Task CategoryPostAsync_IfThrowsExceptionWhenAddingExistingCategory_ReturnOk()
        {
            // Arrange
            global::Spendings.Orchrestrators.Categories.Contracts.InCategory category = new global::Spendings.Orchrestrators.Categories.Contracts.InCategory
            {
                Name = "books"
            };
            global::Spendings.Data.Categories.Category addedCategory = new global::Spendings.Data.Categories.Category
            {
                Name = category.Name
            };
            global::Spendings.Data.Categories.Category categoryFromDB;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                categoryFromDB = DB.Categories.Add(addedCategory).Entity;
                DB.SaveChanges();
            }

            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/Spendings/v1/Categories")
            {
                Content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
            };

            //Act
            var responce = await _client.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);

        }
        private async Task<global::Spendings.Core.Categories.Contracts.Category> ConvertHttpResponceToObj(HttpResponseMessage message)
        {
            var byteResult = await message.Content.ReadAsByteArrayAsync();
            var stringResult = Encoding.UTF8.GetString(byteResult);
            var category = JsonConvert.DeserializeObject<global::Spendings.Core.Categories.Contracts.Category>(stringResult);
            return category;
        }

    }
}
