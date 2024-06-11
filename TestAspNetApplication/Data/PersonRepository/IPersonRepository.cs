using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data
{
    public interface IPersonRepository : IDisposable
    {
        public Task<Person> CreateAsync(Person entity);
        public Task<Person?> DeleteAsync(int id);
        public Task<IEnumerable<Person>> GetAllAsync();
        public Task<Person?> GetByIdAsync(int id);
        public Task<Person?> UpdateAsync(Person entity);

    }
}
