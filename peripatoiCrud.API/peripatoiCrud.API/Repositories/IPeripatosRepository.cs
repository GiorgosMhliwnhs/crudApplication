using Microsoft.AspNetCore.Mvc;
using peripatoiCrud.API.Models.Domain;

namespace peripatoiCrud.API.Repositories
{
    public interface IPeripatosRepository
    {
        Task<Peripatos> CreateAsync(Peripatos peripatos);

        Task<List<Peripatos>> GetAllAsync(string? filter = null, string? filterQuery = null, string? sortBy = null, bool afksousa = true, int arithmosSelidwn = 1, int megethosSelidas = 1000);

        Task<Peripatos?> GetByIdAsync(Guid id);

        Task<Peripatos?> UpdateAsync(Guid id, Peripatos peripatos);

        Task<Peripatos?> DeleteAsync(Guid id);
    }
}
