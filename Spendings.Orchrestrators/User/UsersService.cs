using System.Threading.Tasks;
using Spendings.Core.Users;
using Spendings.Core.Exeptions;

namespace Spendings.Orchrestrators.Users
{
    public class UsersService : IUserService
    {
        private readonly IUserRepository _repo;
        public UsersService(IUserRepository repo)
        {
            _repo = repo;
        }
        public Core.Users.Contracts.User Get(int userId)
        {
            var requestedUser = _repo.Get(userId);
            if(requestedUser == null)
                throw new NotFoundException("No user with given id");

            return requestedUser;
        }
        public Core.Users.Contracts.User Get(Core.Users.Contracts.User user)
        {
            var requestedUser = _repo.Get(user);
            if (requestedUser == null)
                throw new WrongLoginDataException();

            return requestedUser;
        }
        public async Task<Core.Users.Contracts.User> PostAsync(Core.Users.Contracts.User user)
        {
            if (IsUserWithGivenLoginExists(user.Login))
                throw new FailedInsertionException();

            return await _repo.PostAsync(user);
        }
        public async Task<Core.Users.Contracts.User> PatchAsync(int userId, string newLogin)
        {
            bool isExists =  IsUserExists(userId);
            if (isExists)
                throw new NotFoundException("No user with given id");

            return await _repo.PatchAsync(userId, newLogin);
        }   
        public async Task DeleteAsync(int userId)
        {
            bool isExists =  IsUserExists(userId);
            if (isExists)
                throw new AlreadyDeletedException("No user with given id");
            await _repo.DeleteAsync(userId);
        }
        private bool IsUserExists(int userId)
        {
            return  _repo.Get(userId) == null;
        }
        private bool IsUserWithGivenLoginExists(string login)
        {
            return _repo.IsUsersWithLoginExists(login);
        }
    }
}
