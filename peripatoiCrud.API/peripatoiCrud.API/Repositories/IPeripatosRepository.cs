using peripatoiCrud.API.Models.Domain;

namespace peripatoiCrud.API.Repositories
{
    public interface IPeripatosRepository
    {
        Task<Peripatos> CreateAsync(Peripatos peripatos);
    }
}
