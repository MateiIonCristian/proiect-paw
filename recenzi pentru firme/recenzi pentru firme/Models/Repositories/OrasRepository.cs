/*
 * DESCRIERE:
 * Implementarea specifică a depozitului de date (Repository) pentru entitatea Oras.
 * Asigură stocarea, preluarea, editarea și eliminarea înregistrărilor despre orașe din baza de date.
 */

using Microsoft.EntityFrameworkCore;
using recenzi_pentru_firme.Data;
using System.Linq.Expressions;

namespace recenzi_pentru_firme.Models.Repositories;

public class OrasRepository : IRepository<Oras>
{
    private readonly ApplicationDbContext _context;
    public OrasRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Oras>> GetAllAsync(params string[] includes)
    {
        IQueryable<Oras> query = _context.Orase;
        foreach (var include in includes) query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task<Oras?> GetByIdAsync(int id, params string[] includes)
    {
        IQueryable<Oras> query = _context.Orase;
        foreach (var include in includes) query = query.Include(include);
        return await query.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Oras>> SearchAsync(Expression<Func<Oras, bool>> predicate, params string[] includes)
    {
        IQueryable<Oras> query = _context.Orase.Where(predicate);
        foreach (var include in includes) query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task AddAsync(Oras entity) { await _context.Orase.AddAsync(entity); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Oras entity) { _context.Orase.Update(entity); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var item = await _context.Orase.FindAsync(id);
        if (item != null) { _context.Orase.Remove(item); await _context.SaveChangesAsync(); }
    }
}
