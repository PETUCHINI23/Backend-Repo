using Backend.Models;

namespace Backend.Repository;

public interface IBrandRepository : IRepository<Brand>
{
    Task<bool> ExistsByName(string name, int? excludeId = null);
    Task<bool> HasBeers(int brandId);
    Task<IEnumerable<Beer>> GetBeers(int brandId);
}
