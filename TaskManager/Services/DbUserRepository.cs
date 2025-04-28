using TaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Services
{
    public class DbUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _userRepo;

        public DbUserRepository(ApplicationDbContext userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userRepo.Users.FindAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepo.Users.AddAsync(user);
            await _userRepo.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepo.Users.Update(user);
            await _userRepo.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userRepo.Users.FindAsync(id);
            if (user != null)
            {
                _userRepo.Users.Remove(user);
                await _userRepo.SaveChangesAsync();
            }
        }
    }
}
