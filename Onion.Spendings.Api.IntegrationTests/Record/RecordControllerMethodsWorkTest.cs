using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Onion.Spendings.Api.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Spendings.Data.DB;
using System.Linq;

namespace Onion.Spendings.Api.Tests.Records
{
    public class RecordControllerMethodsWorkTest :
    IClassFixture<CustomWebApplicationFactory<onion_spendings.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<onion_spendings.Startup>
            _factory;
        public RecordControllerMethodsWorkTest(
            CustomWebApplicationFactory<onion_spendings.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task PostAsync_IfMethodWorks_ReturnOk()
        {
            // Arrange
            int userId = 1;
            global::Spendings.Orchrestrators.Records.Contracts.InRecord record = new global::Spendings.Orchrestrators.Records.Contracts.InRecord
            {
                Date = new DateTime(2005, 10, 9),
                CategoryId = 1,
                Amount = 1000
            };
            var request = new HttpRequestMessage(HttpMethod.Post, $"/Spendings/v1/Users/{userId}/Records")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                       record),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var responce = await _client.SendAsync(request);

            // Assert
            responce.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);

        }

        [Fact]
        public async Task GetListAsync_IfMethodWorks_ReturnCorrectList()
        {
            // Arrange
            int categoryId = 1;
            int userId = 1;
            int amount = 1000;
            DateTime firstInsertedDate = new DateTime(2004, 10, 20);
            DateTime secondInsertedDate = new DateTime(2004, 10, 23);
            DateTime endDate = new DateTime(2004, 10, 26);
            global::Spendings.Data.Records.Record firstRecord = new global::Spendings.Data.Records.Record
            {
                Date = firstInsertedDate,
                CategoryId = categoryId,
                Amount = amount,
                UserId = userId
            };
            global::Spendings.Data.Records.Record secondRecord = new global::Spendings.Data.Records.Record
            {
                Date = secondInsertedDate,
                CategoryId = categoryId,
                Amount = amount,
                UserId = userId
            };
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DB.Records.AddRange(firstRecord, secondRecord);
                DB.SaveChanges();
            };
            var GetRequest = new HttpRequestMessage(HttpMethod.Get, $"/Spendings/v1/Users/{userId}/Records/from/{firstInsertedDate.ToString("yyyy-MM-dd")}/till/{endDate.ToString("yyyy-MM-dd")}");
            //Act

            var getResponce =  await _client.SendAsync(GetRequest);
            var records = await GetModelFromHttpResponce.GetList<global::Spendings.Orchrestrators.Records.Contracts.OutRecord>(getResponce);
            
            // Assert
            getResponce.EnsureSuccessStatusCode();
            Assert.Equal(2, records.Count);
            Assert.Equal(firstRecord.Date, records[0].Date);
            Assert.Equal(secondRecord.Date, records[1].Date);
        }
        [Fact]
        public async Task GetListAsync_IfPaginatingCorrectly_ReturnCorrectList()
        {
            // Arrange
            int userId = 1;
            int amount = 1000;
            DateTime firstInsertedDate = new DateTime(2000, 10, 20);
            DateTime secondInsertedDate = new DateTime(2000, 10, 23);
            DateTime endDate = new DateTime(2000, 10, 26);
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                for(int i = 1;i <= 100; i++)
                {
                    DB.Records.Add(new global::Spendings.Data.Records.Record
                    {
                        Date = secondInsertedDate,
                        CategoryId = i,
                        Amount = amount,
                        UserId = userId
                    });
                }
                DB.SaveChanges();
            };
            int PageSize = 20;
            int PageNumber = 3;
            var GetRequest = new HttpRequestMessage(HttpMethod.Get, $"/Spendings/v1/Users/{userId}/Records/from/{firstInsertedDate.ToString("yyyy-MM-dd")}/till/{endDate.ToString("yyyy-MM-dd")}?PageNumber={PageNumber}&PageSize={PageSize}");
            //Act

            var getResponce = await _client.SendAsync(GetRequest);
            var records = await GetModelFromHttpResponce.GetList<global::Spendings.Orchrestrators.Records.Contracts.OutRecord>(getResponce);

            // Assert
            getResponce.EnsureSuccessStatusCode();
            Assert.Equal(PageSize, records.Count);
            Assert.Equal(41, records[0].CategoryId);
            Assert.Equal(60, records.Last().CategoryId);
        }
        [Fact]
        public async Task DeleteAsync_IfCantFindRecordWithId_ReturnNotFound()
        {
            // Arrange
            global::Spendings.Data.Records.Record postRecord = new global::Spendings.Data.Records.Record
            {
                Date = new DateTime(2005, 8, 20),
                CategoryId = 1,
                Amount = 1000
            };
            global::Spendings.Data.Records.Record postedRecord;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                postedRecord = DB.Records.Add(postRecord).Entity;
                DB.SaveChanges();
            };

            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"/Spendings/v1/Records/{postedRecord.Id}");
            //Act

            var deleteResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"/Spendings/v1/Records/{postedRecord.Id}"));
            var responce = await _client.SendAsync(getRequest);

            // Assert
            deleteResponce.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NotFound, responce.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_IfRecordUpdated_ReturnOk()
        {
            // Arrange
            global::Spendings.Data.Records.Record postRecord = new global::Spendings.Data.Records.Record
            {
                Date = new DateTime(2004, 8, 20),
                CategoryId = 1,
                Amount = 1000
            };
            global::Spendings.Data.Records.Record newRecord = new global::Spendings.Data.Records.Record
            {
                Date = new DateTime(2004, 8, 20),
                CategoryId = 2,
                Amount = 10000
            };
            global::Spendings.Data.Records.Record postedRecord;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                postedRecord = DB.Records.Add(postRecord).Entity;
                DB.SaveChanges();
            };

            var updateRequest = new HttpRequestMessage(HttpMethod.Put, $"/Spendings/v1/Records/{postedRecord.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                      newRecord),
                   Encoding.UTF8,
                   "application/json")
            };

            //Act
            var updatedResponce = await _client.SendAsync(updateRequest);
            global::Spendings.Data.Records.Record recordAfterUpdate;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                recordAfterUpdate = DB.Records.Find(postedRecord.Id);
            };

            // Assert
            updatedResponce.EnsureSuccessStatusCode();
            Assert.Equal(newRecord.CategoryId, recordAfterUpdate.CategoryId);
            Assert.Equal(newRecord.Amount, recordAfterUpdate.Amount);
        }
       
        [Fact]
        public async Task PatchAsync_IfRecordPatched_ReturnPatchedRecord()
        {
            // Arrange
            int addedAmount = 10000;

            global::Spendings.Data.Records.Record postRecord = new global::Spendings.Data.Records.Record
            {
                Date = new DateTime(2006, 8, 20),
                CategoryId = 1,
                Amount = 1000
            };
            global::Spendings.Data.Records.Record postedRecord;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                postedRecord = DB.Records.Add(postRecord).Entity;
                DB.SaveChanges();
            };

            var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"/Spendings/v1/Records/{postedRecord.Id}?additionalAmount={addedAmount}");
            //Act
            var patchResponce = await _client.SendAsync(patchRequest);
            global::Spendings.Data.Records.Record recordAfterPatch;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                recordAfterPatch = DB.Records.Find(postedRecord.Id);
            };
            
            // Assert
            patchResponce.EnsureSuccessStatusCode();
            Assert.Equal(addedAmount + postRecord.Amount, recordAfterPatch.Amount);
            Assert.Equal(postRecord.CategoryId, recordAfterPatch.CategoryId);
        }
        [Fact]
        public async Task RecordPatchAsync_IfAmountOverflowed_ReturnBadRequest()
        {
            // Arrange
            global::Spendings.Data.Records.Record postRecord = new global::Spendings.Data.Records.Record
            {
                Date = new DateTime(2005, 8, 20),
                CategoryId = 1,
                Amount = 1000
            };
            global::Spendings.Data.Records.Record postedRecord;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                postedRecord = DB.Records.Add(postRecord).Entity;
                DB.SaveChanges();
            };

            int newAmount = int.MaxValue;

            var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"/Spendings/v1/Records/{postedRecord.Id}?additionalAmount={newAmount}");
            //Act

            var responce = await _client.SendAsync(patchRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }
        [Fact]
        public async Task RecordDeleteListAsync_IfWorks_ReturnOk()
        {
            // Arrange
            int categoryId = 1;
            int userId = 1;
            int amount = 1000;
            DateTime firstInsertedDate = new DateTime(2001, 10, 20);
            DateTime secondInsertedDate = new DateTime(2001, 10, 23);
            DateTime endDate = new DateTime(2001, 10, 26);
            global::Spendings.Data.Records.Record firstRecord = new global::Spendings.Data.Records.Record
            {
                Date = firstInsertedDate,
                CategoryId = categoryId,
                Amount = amount
            };
            global::Spendings.Data.Records.Record secondRecord = new global::Spendings.Data.Records.Record
            {
                Date = secondInsertedDate,
                CategoryId = categoryId,
                Amount = amount
            };
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DB.Records.AddRange(firstRecord, secondRecord);
                DB.SaveChanges();
            };

            var DeleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/Spendings/v1/Users/{userId}/Records/from/{firstInsertedDate.ToString("yyyy-MM-dd")}/till/{endDate.ToString("yyyy-MM-dd")}");

            //Act
            var DeleteResponce = await _client.SendAsync(DeleteRequest);
            List<global::Spendings.Data.Records.Record> records;
            using (var scope = _factory.serviceProvider.CreateScope())
            {
                var DB = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                records = DB.Records.Where(r => r.Date >= firstInsertedDate && r.Date <= endDate && r.UserId == userId).ToList();
            };

            // Assert
            DeleteResponce.EnsureSuccessStatusCode();
            Assert.Empty(records);
        }
    }
}
