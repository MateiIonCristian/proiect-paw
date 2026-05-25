/*
 * DESCRIERE:
 * Serviciu care implementează logica de business pentru operațiile legate de orașe,
 * gestionând conexiunea dintre controlerul de orașe și repository-ul corespunzător.
 */

using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Models.Repositories;

namespace recenzi_pentru_firme.Services;

public class OrasService
{
    private readonly IRepository<Oras> _repo;

    public OrasService(IRepository<Oras> repo) => _repo = repo;

    public async Task<IEnumerable<Oras>> GetToateOrasele() => await _repo.GetAllAsync();

    public async Task<Oras?> GetOrasById(int id) => await _repo.GetByIdAsync(id);

    public async Task AdaugaOras(Oras oras) => await _repo.AddAsync(oras);

    public async Task UpdateOras(Oras oras) => await _repo.UpdateAsync(oras);

    public async Task StergeOras(int id) => await _repo.DeleteAsync(id);
}
