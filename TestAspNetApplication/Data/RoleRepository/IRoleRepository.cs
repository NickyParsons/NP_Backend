using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data
{
    public interface IRoleRepository : IDisposable
    {
        public Task<IEnumerable<Role>> GetAllRoles();
        public Task<Role?> GetRoleById(int id);
        public Task<Role?> GetRoleByName(string name);
        public Task<Role> CreateRole(string newRoleName);
        public Task<Role?> EditRole(Role editedRole);
        public Task<Role?> DeleteRole(int id);
    }
}
