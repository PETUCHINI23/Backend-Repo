using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class BrandRepository : IBrandRepository
{
    private readonly StoreContext _context;

    public BrandRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Brand>> Get() =>
        await _context.Brands.ToListAsync();

    public async Task<Brand?> GetById(int id) =>
        await _context.Brands.FindAsync(id);

    public async Task Add(Brand entity) =>
        await _context.Brands.AddAsync(entity);

    public void Update(Brand entity)
    {
        _context.Brands.Attach(entity);
        _context.Brands.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(Brand entity) =>
        _context.Brands.Remove(entity);

    public async Task Save() =>
        await _context.SaveChangesAsync();

    public async Task<bool> ExistsByName(string name, int? excludeId = null) =>
        await _context.Brands.AnyAsync(b => b.Name == name && (excludeId == null || b.BrandId != excludeId));

    public async Task<bool> HasBeers(int brandId) =>
        await _context.Beers.AnyAsync(b => b.BrandId == brandId);

    public async Task<IEnumerable<Beer>> GetBeers(int brandId) =>
        await _context.Beers.Where(b => b.BrandId == brandId).ToListAsync();
}
