/*
 * DESCRIERE:
 * Implementarea specifică a depozitului de date (Repository) pentru entitatea Categorie.
 * Gestionează interogările direct în baza de date prin intermediul Entity Framework Core și ApplicationDbContext.
 */

using Microsoft.EntityFrameworkCore;
using recenzi_pentru_firme.Data;
using System.Linq.Expressions;

namespace recenzi_pentru_firme.Models.Repositories;

public class CategorieRepository : IRepository<Categorie>
{
    private readonly ApplicationDbContext _context;
    public CategorieRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Categorie>> GetAllAsync(params string[] includes)
    {
        IQueryable<Categorie> query = _context.Categorii;
        foreach (var include in includes) query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task<Categorie?> GetByIdAsync(int id, params string[] includes)
    {
        IQueryable<Categorie> query = _context.Categorii;
        foreach (var include in includes) query = query.Include(include);
        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Categorie>> SearchAsync(Expression<Func<Categorie, bool>> predicate, params string[] includes)
    {
        IQueryable<Categorie> query = _context.Categorii.Where(predicate);
        foreach (var include in includes) query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task AddAsync(Categorie entity) { await _context.Categorii.AddAsync(entity); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Categorie entity) { _context.Categorii.Update(entity); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var item = await _context.Categorii.FindAsync(id);
        if (item != null) { _context.Categorii.Remove(item); await _context.SaveChangesAsync(); }
    }
}
