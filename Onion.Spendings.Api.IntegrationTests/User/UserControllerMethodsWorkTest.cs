using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Onion.Spendings.Api.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Spendings.Data.DB;
using Spendings.Orchrestrators.Users.Contracts;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Onion.Spendings.Api.Tests.Users
{
    public class UserControllerMethodsWorkTest
    :
    IClassFixture<CustomWebApplicationFactory<onion_spendings.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<onion_spendings.Startup>
            _factory;

        public UserControllerMethodsWorkTest(
            CustomWebApplicationFactory<onion_spendings.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAsync_IfWorks_ReturnUser()
        {
            // Arrange
            var addedUser = new global::Spendings.Data.Users.User
            {
                Login = "dedadd",
                Password = "asdasd",
                IsDeleted = false
            };

            global::Spendings.Data.Users.User userFromDB;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                userFromDB = DB.Users.Add(addedUser).Entity;
                DB.SaveChanges();
            }
            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"/Spendings/v1/Users/{userFromDB.Id}");

            //Act

            var getResponse = await _client.SendAsync(getRequest);
            var user = await GetModelFromHttpResponce.GetSingle<OutUser>(getResponse);

            // Assert
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(userFromDB.Login, user.Login);
            Assert.Equal(userFromDB.Password, user.Password);
        }

        [Fact]
        public async Task GetAsync_IfIdIsUndefined_ReturnBadRequest()
        {
            // Arrange
            int undefinedId = 99999;

            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"/Spendings/v1/Users/{undefinedId}");
            //Act

            var responce = await _client.SendAsync(getRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task PostAsync_IfWorks_ReturnPostedUser()
        {
            // Arrange
            var postingUser = new InUser
            {
                Password = "sdasdasda",
                Login = "sdasdad"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/Spendings/v1/Users")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                    postingUser),
                    Encoding.UTF8, "application/json")
            };

            //Act
            var response = await _client.SendAsync(request);
            var user = await GetModelFromHttpResponce.GetSingle<OutUser>(response);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(postingUser.Login, user.Login);
            Assert.Equal(postingUser.Password, user.Password);
        }

        [Fact]
        public async Task PatcAsync_IfLoginOfGivenUserChanged_ReturnPatchedUser()
        {
            
            // Arrange
            var addedUser = new global::Spendings.Data.Users.User
            {
                Password = "password",
                Login = "login"
            };
            string newLogin = "asdasLLLL";
            global::Spendings.Data.Users.User userFromDB;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                userFromDB = DB.Users.Add(addedUser).Entity;
                DB.SaveChanges();
            }


            //Act
            var patchResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Patch, $"/Spendings/v1/Users/{userFromDB.Id}?newLogin={newLogin}"));
            var userReturnedByController = await GetModelFromHttpResponce.GetSingle<OutUser>(patchResponce);

            global::Spendings.Data.Users.User userFromDbAfterPatch;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                userFromDbAfterPatch = DB.Users.Find(userFromDB.Id);
            }
            // Assert
            patchResponce.EnsureSuccessStatusCode();

            Assert.Equal(userFromDbAfterPatch.Login, userReturnedByController.Login);
            Assert.Equal(userFromDbAfterPatch.Login, newLogin);
            Assert.Equal(userReturnedByController.Login, newLogin);
        }

        [Fact]
        public async Task PostUser_IfUserWithLoginExists_ReturnBadRequest()
        {
            // Arrange
            var addedUser = new global::Spendings.Data.Users.User
            {
                Password = "password",
                Login = "loGGin"
            };
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DB.Users.Add(addedUser);
                DB.SaveChanges();
            }

            var getRequest = new HttpRequestMessage(HttpMethod.Post, "/Spendings/v1/Users")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                        new InUser
                        {
                            Password = addedUser.Password,
                            Login = addedUser.Login
                        }),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var responce = await _client.SendAsync(getRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_IfDeletingDeletedUser_ReturnBadRequest()
        {
            // Arrange
            var addedUser = new global::Spendings.Data.Users.User
            {
                Password = "password",
                Login = "loGGgin"
            };
            global::Spendings.Data.Users.User userFromDB;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                userFromDB = DB.Users.Add(addedUser).Entity;
                DB.SaveChanges();
            }

            var firstDeleteRequest =  new HttpRequestMessage(HttpMethod.Delete, $"/Spendings/v1/Users/{addedUser.Id}");
            var secondDeleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/Spendings/v1/Users/{addedUser.Id}");
            //Act
            var deleteResponce = await _client.SendAsync(firstDeleteRequest);
  
            var responce =  await _client.SendAsync(secondDeleteRequest);

            // Assert
            deleteResponce.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task GetAndPatchDeletedUser_IfThrowsException_ReturnOk()
        {
            // Arrange
            var addedUser = new global::Spendings.Data.Users.User
            {
                Password = "password",
                Login = "loGsGgin"
            };

            global::Spendings.Data.Users.User userFromDB;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                userFromDB = DB.Users.Add(addedUser).Entity;
                DB.SaveChanges();
            }
            var DeleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/Spendings/v1/Users/{addedUser.Id}");
            var GetRequest = new HttpRequestMessage(HttpMethod.Get, $"/Spendings/v1/Users/{addedUser.Id}");
            var PatchRequest = new HttpRequestMessage(HttpMethod.Patch, $"/Spendings/v1/Users/{userFromDB.Id}?newLogin=asdasdass");
            //Act
            var deleteResponce = await _client.SendAsync(DeleteRequest);

            var postResponce = await _client.SendAsync(GetRequest);
            var patchResponce =  await _client.SendAsync(PatchRequest);

            // Assert
            deleteResponce.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.BadRequest, postResponce.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, patchResponce.StatusCode);
        }
        [Fact]
        public async Task GetUser_IfCantfind_ReturnBadRequest()
        {
            // Arrange
            var addedUser = new global::Spendings.Data.Users.User
            {
                Password = "password",
                Login = "loGsGGGgin"
            };
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DB.Users.Add(addedUser);
                DB.SaveChanges();
            }

            var askedUser = new InUser
            {
                Password = "1231dfaeg",
                Login = "000000"
            };
           
            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"/Spendings/v1/Users?Login={askedUser.Login}&Password={askedUser.Password}");

            //Act
            var responce = await _client.SendAsync(getRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }
    }
}