
using System.Threading.Tasks;

namespace Spendings.Core.Users
{
    public interface IUserRepository
    {
        Contracts.User Get(int userId);
        Contracts.User Get(Contracts.User user);
        Task<Contracts.User> PostAsync(Contracts.User user);
        Task<Contracts.User> PatchAsync(int userId,string newUser);
        Task DeleteAsync(int userId);

        bool IsUsersWithLoginExists(string login);
    }
}
