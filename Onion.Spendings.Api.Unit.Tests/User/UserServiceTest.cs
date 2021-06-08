using System.Threading.Tasks;
using Moq;
using Spendings.Core.Users;
using Spendings.Orchrestrators.Users;
using Spendings.Core.Exeptions;
using Xunit;

namespace Onion.Spendings.Api.Unit.Tests.User
{
    public class UserServiceTest
    {
        private readonly UsersService _service;
        private readonly Mock<IUserRepository> _mockedRepository = new Mock<IUserRepository>();

        public UserServiceTest()
        {
            _service = new UsersService(_mockedRepository.Object);
        }

        [Fact]
        public async Task PostAsync_IfLoginIsNotOriginal_ThrowFailedInsertionException()
        {
            //Arrange
            string login = "existing";
            global::Spendings.Core.Users.Contracts.User user = new global::Spendings.Core.Users.Contracts.User
            {
                Login = login,
                Password = "asdeasd"
            };

            _mockedRepository.Setup(r => r.IsUsersWithLoginExists(login)).Returns(true);

            //Act
            var exception = await Assert.ThrowsAsync<FailedInsertionException>(async () => await _service.PostAsync(user));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task PatchAsync_IfIdUndefined_ThrowNotFoundException()
        {
            //Arrange
            string login = "existing";
            int requestedId = 100;
            global::Spendings.Core.Users.Contracts.User returnedByRepository = null;
            _mockedRepository.Setup(r => r.Get(requestedId)).Returns(returnedByRepository);

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _service.PatchAsync(requestedId, login));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task DeleteAsync_IfIdUndefined_ThrowAlreadyDeletedException()
        {
            //Arrange
            int requestedId = 100;
            global::Spendings.Core.Users.Contracts.User returnedByRepository = null;
            _mockedRepository.Setup(r => r.Get(requestedId)).Returns(returnedByRepository);

            //Act
            var exception = await Assert.ThrowsAsync<AlreadyDeletedException>(async () => await _service.DeleteAsync(requestedId));

            //Assert
            Assert.NotNull(exception);
        }
        [Fact]
        public async Task GetByIdAsync_IfIdUndefined_ThrowNotFoundException()
        {
            //Arrange
            int requestedId = 100;
            global::Spendings.Core.Users.Contracts.User returnedByRepository = null;
            _mockedRepository.Setup(r => r.Get(requestedId)).Returns(returnedByRepository);
            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => _service.Get(requestedId));

            //Assert
            Assert.NotNull(exception);
        }
        [Fact]
        public async Task GetByLoginAsync_IfIdUndefined_ThrowWrongLoginDataException()
        {
            //Arrange
            global::Spendings.Core.Users.Contracts.User user = new global::Spendings.Core.Users.Contracts.User
            {
                Login = "asdasdas",
                Password = "asdeasd"
            };
            
            global::Spendings.Core.Users.Contracts.User returnedByRepository = null;
            _mockedRepository.Setup(r => r.Get(user)).Returns(returnedByRepository);

            //Act
            var exception = await Assert.ThrowsAsync<WrongLoginDataException>(async () => _service.Get(user));

            //Assert
            Assert.NotNull(exception);
        }
    }
}
