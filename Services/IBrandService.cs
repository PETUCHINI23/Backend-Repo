using Backend.DTOs;

namespace Backend.Services;

public interface IBrandService : ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto>
{
    Task<IEnumerable<BeerDto>?> GetBeers(int id);
}
