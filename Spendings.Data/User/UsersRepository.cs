using AutoMapper;
using Spendings.Data.DB;
using Spendings.Core.Users;
using System.Threading.Tasks;
using System.Linq;

namespace Spendings.Data.Users
{
    public class UsersRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UsersRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public  Core.Users.Contracts.User Get(int userId)
        {
            User user =  _context.Users.First(u => u.IsDeleted == false && u.Id == userId);        
            return _mapper.Map<Core.Users.Contracts.User>(user);
        }
        public  Core.Users.Contracts.User Get(Core.Users.Contracts.User user)
        {
            var requestedUser = _context.Users.First(u => u.Login == user.Login && u.Password == user.Password && u.IsDeleted == false);
            return _mapper.Map<Core.Users.Contracts.User>(requestedUser);
        }
        public async Task<Core.Users.Contracts.User> PostAsync(Core.Users.Contracts.User user)
        {
            var mappedUser = _mapper.Map<User>(user);
            var addedUser = await _context.Users.AddAsync(mappedUser);
            await _context.SaveChangesAsync();
            return _mapper.Map<Core.Users.Contracts.User>(addedUser.Entity);
        }

        public async Task<Core.Users.Contracts.User> PatchAsync(int userId, string newLogin)
        {
            User userToPatch = _context.Users.First(u => u.IsDeleted == false && u.Id == userId);
            userToPatch.Login = newLogin;
            var patchedUser = _context.Users.Update(userToPatch).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Users.Contracts.User>(patchedUser);
        }
        public async Task DeleteAsync(int userId)
        {
            User user = await _context.Users.FindAsync(userId);
            user.IsDeleted = true;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public bool IsUsersWithLoginExists(string login)
        {
            return _context.Users.Count(c => c.Login == login && c.IsDeleted == false) > 0;
        }
    }

}