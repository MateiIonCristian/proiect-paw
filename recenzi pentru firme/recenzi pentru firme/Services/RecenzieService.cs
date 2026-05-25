/*
 * DESCRIERE:
 * Serviciu care implementează logica de business pentru recenzii, facilitând adăugarea,
 * actualizarea și ștergerea comentariilor și ratingurilor oferite de utilizatori pentru diverse firme.
 */

using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Models.Repositories;

namespace recenzi_pentru_firme.Services;

public class RecenzieService
{
    private readonly IRepository<Recenzie> _repo;

    public RecenzieService(IRepository<Recenzie> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Recenzie>> GetToateRecenziile() => await _repo.GetAllAsync("Firma");

    public async Task<Recenzie?> GetRecenzieById(int id) => await _repo.GetByIdAsync(id, "Firma");

    public async Task AdaugaRecenzie(Recenzie rec) => await _repo.AddAsync(rec);
    public async Task UpdateRecenzie(Recenzie rec) => await _repo.UpdateAsync(rec);
    public async Task StergeRecenzie(int id) => await _repo.DeleteAsync(id);
}
