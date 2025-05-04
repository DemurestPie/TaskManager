using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class DbProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _db;

        public DbProjectRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _db.Projects
                .Include(p => p.Tasks)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _db.Projects
                .Include(p => p.Tasks)
                .ThenInclude(t => t.User)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task AddProjectAsync(Project project)
        {
            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _db.Projects.Update(project);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await GetProjectByIdAsync(id);
            if (project != null)
            {
                _db.Projects.Remove(project);
                await _db.SaveChangesAsync();
            }
        }
    }
}
