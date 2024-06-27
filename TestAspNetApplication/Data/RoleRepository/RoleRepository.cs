using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data
{
    public class RoleRepository : IRoleRepository
    {
        private PosgresDbContext _dbContext;
        private ILogger<RoleRepository> _logger;
        public RoleRepository(ILogger<RoleRepository> logger, PosgresDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleById(int id)
        {
            Role? dbRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRole == null)
            {
                _logger.LogWarning($"Role with id \'{id}\' not found");
            }
            return dbRole;
        }

        public async Task<Role?> GetRoleByName(string name)
        {
            Role? dbRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (dbRole == null)
            {
                _logger.LogWarning($"Role \'{name}\' not found");
            }
            return dbRole;
        }

        public async Task<Role> CreateRole(string newRoleName)
        {
            Role newRole = new Role();
            newRole.Name = newRoleName;
            newRole.Description = "Undefined";
            _dbContext.Roles.Add(newRole);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug($"User \'{newRole.Name}\' created");
            return newRole;
        }
        public async Task<Role> CreateRole(Role newRole)
        {
            await _dbContext.Roles.AddAsync(newRole);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug($"User \'{newRole.Name}\' created");
            return newRole;
        }
        public async Task<Role?> EditRole(Role editedRole)
        {
            Role? dbRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == editedRole.Id);
            if (dbRole != null)
            {
                dbRole.Name = editedRole.Name;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Role with id \'{editedRole.Id}\' not found");
            }
            return dbRole;
        }
        public async Task<Role?> DeleteRole(int id)
        {
            Role? dbRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRole != null)
            {
                _dbContext.Roles.Remove(dbRole);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Role with id \'{id}\' not found");
            }
            return dbRole;
        }
    }
}
