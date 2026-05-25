/*
 * DESCRIERE:
 * Interfață generică (Generic Repository Pattern) care definește contractul pentru operațiile fundamentale de acces la date (CRUD),
 * oferind suport pentru specificarea dinamică a proprietăților de navigare relaționate (Eager Loading) și filtrare prin expresii lambda.
 */

using System.Linq.Expressions;

namespace recenzi_pentru_firme.Models.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(params string[] includes);
    Task<T?> GetByIdAsync(int id, params string[] includes);
    Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate, params string[] includes);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
