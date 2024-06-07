using peripatoiCrud.API.Models.Domain;

namespace peripatoiCrud.API.Repositories
{
    public interface IPerioxhRepository
    {
        Task<List<Perioxh>> GetAllAsync();

        Task<Perioxh?> GetByIdAsync(Guid id);

        Task<Perioxh> CreateAsync(Perioxh perioxh);

        Task<Perioxh?> UpdateAsync(Guid id, Perioxh perioxh);

        Task<Perioxh?> DeleteAsync(Guid id);
    }
}
