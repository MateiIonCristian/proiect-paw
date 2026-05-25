/*
 * DESCRIERE:
 * Implementarea specifică a depozitului de date (Repository) pentru entitatea Recenzie.
 * Permite inserarea, interogarea, actualizarea și ștergerea recenziilor și evaluărilor utilizatorilor.
 */

using Microsoft.EntityFrameworkCore;
using recenzi_pentru_firme.Data;
using System.Linq.Expressions;

namespace recenzi_pentru_firme.Models.Repositories;

public class RecenzieRepository : IRepository<Recenzie>
{
    private readonly ApplicationDbContext _context;
    public RecenzieRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Recenzie>> GetAllAsync(params string[] includes)
    {
        IQueryable<Recenzie> query = _context.Recenzii;
        foreach (var include in includes) query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task<Recenzie?> GetByIdAsync(int id, params string[] includes)
    {
        IQueryable<Recenzie> query = _context.Recenzii;
        foreach (var include in includes) query = query.Include(include);
        return await query.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Recenzie>> SearchAsync(Expression<Func<Recenzie, bool>> predicate, params string[] includes)
    {
        IQueryable<Recenzie> query = _context.Recenzii.Where(predicate);
        foreach (var include in includes) query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task AddAsync(Recenzie entity) { await _context.Recenzii.AddAsync(entity); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Recenzie entity) { _context.Recenzii.Update(entity); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var item = await _context.Recenzii.FindAsync(id);
        if (item != null) { _context.Recenzii.Remove(item); await _context.SaveChangesAsync(); }
    }
}
