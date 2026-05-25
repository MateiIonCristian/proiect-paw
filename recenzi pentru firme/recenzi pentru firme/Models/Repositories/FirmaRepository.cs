/*
 * DESCRIERE:
 * Implementarea specifică a depozitului de date (Repository) pentru entitatea Firma.
 * Permite accesarea, inserarea, modificarea și ștergerea datelor specifice firmelor din baza de date.
 */

using Microsoft.EntityFrameworkCore;
using recenzi_pentru_firme.Data;
using System.Linq.Expressions;

namespace recenzi_pentru_firme.Models.Repositories;

public class FirmaRepository : IRepository<Firma>
{
    private readonly ApplicationDbContext _context;
    public FirmaRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Firma>> GetAllAsync(params string[] includes)
    {
        IQueryable<Firma> query = _context.Firme;
        foreach (var include in includes) query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task<Firma?> GetByIdAsync(int id, params string[] includes)
    {
        IQueryable<Firma> query = _context.Firme;
        foreach (var include in includes) query = query.Include(include);
        return await query.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Firma>> SearchAsync(Expression<Func<Firma, bool>> predicate, params string[] includes)
    {
        IQueryable<Firma> query = _context.Firme.Where(predicate);
        foreach (var include in includes) query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task AddAsync(Firma entity) { await _context.Firme.AddAsync(entity); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Firma entity) { _context.Firme.Update(entity); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var item = await _context.Firme.FindAsync(id);
        if (item != null) { _context.Firme.Remove(item); await _context.SaveChangesAsync(); }
    }
}
