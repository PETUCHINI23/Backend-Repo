using Backend.DTOs;
using Backend.Exceptions;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;

    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<IEnumerable<BrandDto>> Get()
    {
        var brands = await _brandRepository.Get();
        return brands.Select(ToDto);
    }

    public async Task<BrandDto?> GetById(int id)
    {
        var brand = await _brandRepository.GetById(id);
        return brand == null ? null : ToDto(brand);
    }

    public async Task<IEnumerable<BeerDto>?> GetBeers(int id)
    {
        var brand = await _brandRepository.GetById(id);
        if (brand == null)
        {
            return null;
        }

        var beers = await _brandRepository.GetBeers(id);
        return beers.Select(b => new BeerDto
        {
            Id = b.BeerId,
            Name = b.Name,
            BrandId = b.BrandId,
            Alcohol = b.Alcohol
        });
    }

    public async Task<BrandDto> Add(BrandInsertDto brandInsertDto)
    {
        var brand = new Brand
        {
            Name = brandInsertDto.Name
        };

        await _brandRepository.Add(brand);
        await _brandRepository.Save();

        return ToDto(brand);
    }

    public async Task<BrandDto?> Update(int id, BrandUpdateDto brandUpdateDto)
    {
        var brand = await _brandRepository.GetById(id);
        if (brand == null)
        {
            return null;
        }

        brand.Name = brandUpdateDto.Name;
        _brandRepository.Update(brand);
        await _brandRepository.Save();

        return ToDto(brand);
    }

    public async Task<BrandDto?> Delete(int id)
    {
        var brand = await _brandRepository.GetById(id);
        if (brand == null)
        {
            return null;
        }

        if (await _brandRepository.HasBeers(id))
        {
            throw new ConflictException("No se puede eliminar la marca porque tiene cervezas asociadas");
        }

        _brandRepository.Delete(brand);
        await _brandRepository.Save();

        return ToDto(brand);
    }

    private static BrandDto ToDto(Brand brand) => new()
    {
        Id = brand.BrandId,
        Name = brand.Name
    };
}
