using System.Threading.Tasks;
using Moq;
using Spendings.Core.Records;
using Spendings.Orchrestrators.Records;
using Spendings.Core.Exeptions;
using Xunit;

namespace Onion.Spendings.Api.Unit.Tests.Record
{
    public class RecordServiceTest
    {
        private readonly RecordsService _service;
        private readonly Mock<IRecordRepository> _mockedRepository = new Mock<IRecordRepository>();

        public RecordServiceTest()
        {
            _service = new RecordsService(_mockedRepository.Object);
        }

        [Fact]
        public async Task GetAsync_IfIdIsUndefined_ThrowNotFoundException()
        {
            //Arrange
            int recordId = 10;
            global::Spendings.Core.Records.Contracts.Record record = null;

            _mockedRepository.Setup(r => r.GetAsync(recordId)).ReturnsAsync(record);
            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetAsync(recordId));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task UpdateAsync_IfIdIsUndefined_ThrowNotFoundException()
        {
            //Arrange
            int recordId = 10;
            global::Spendings.Core.Records.Contracts.Record update = new global::Spendings.Core.Records.Contracts.Record
            {
                CategoryId = 1,
                Amount = 100,
                UserId = 1
            };
            global::Spendings.Core.Records.Contracts.Record record = null;

            _mockedRepository.Setup(r => r.GetAsync(recordId)).ReturnsAsync(record);
            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _service.UpdateAsync(update, recordId));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task PatchAsync_IfIdIsUndefined_ThrowNotFoundException()
        {
            //Arrange
            int recordId = 10;
            int addedAmount = 1000;

            global::Spendings.Core.Records.Contracts.Record record = null;

            _mockedRepository.Setup(r => r.GetAsync(recordId)).ReturnsAsync(record);
            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _service.PatchAsync(addedAmount, recordId));

            //Assert
            Assert.NotNull(exception);
        }
        [Fact]
        public async Task PatchAsync_IfOverflowed_ThrowOverflowException()
        {
            //Arrange
            int recordId = 10;
            int addedAmount = int.MaxValue;

            global::Spendings.Core.Records.Contracts.Record record = new global::Spendings.Core.Records.Contracts.Record
            { 
                Amount = int.MaxValue
            };

            _mockedRepository.Setup(r => r.GetAsync(recordId)).ReturnsAsync(record);
            //Act
            var exception = await Assert.ThrowsAsync<System.OverflowException>(async () => await _service.PatchAsync(addedAmount, recordId));

            //Assert
            Assert.NotNull(exception);
        }
        [Fact]
        public async Task DeleteAsync_IfIdIsUndefined_ThrowDeletionFailedException()
        {
            //Arrange
            int recordId = 10;
            global::Spendings.Core.Records.Contracts.Record record = null;

            _mockedRepository.Setup(r => r.GetAsync(recordId)).ReturnsAsync(record);
            //Act
            var exception = await Assert.ThrowsAsync<DeletionFailedException>(async () => await _service.DeleteAsync(recordId));

            //Assert
            Assert.NotNull(exception);
        }

    }
}
