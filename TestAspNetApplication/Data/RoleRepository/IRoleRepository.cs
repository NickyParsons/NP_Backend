using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data
{
    public interface IRoleRepository
    {
        public Task<IEnumerable<Role>> GetAllRoles();
        public Task<Role?> GetRoleById(Guid id);
        public Task<Role?> GetRoleByName(string name);
        public Task<Role> CreateRole(string newRoleName);
        public Task<Role> CreateRole(Role newRole);
        public Task<Role?> EditRole(Role editedRole);
        public Task<Role?> DeleteRole(Guid id);
    }
}
