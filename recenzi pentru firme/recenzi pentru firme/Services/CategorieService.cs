/*
 * DESCRIERE:
 * Serviciu care implementează logica de business pentru operațiile legate de categorii de firme,
 * intermediind comunicarea dintre controlere și depozitul de date (repository).
 */

using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Models.Repositories;

namespace recenzi_pentru_firme.Services;

public class CategorieService
{
    private readonly IRepository<Categorie> _repo;

    public CategorieService(IRepository<Categorie> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Categorie>> GetToateCategoriile() => await _repo.GetAllAsync();

    public async Task<Categorie?> GetCategorieById(int id) => await _repo.GetByIdAsync(id);

    public async Task AdaugaCategorie(Categorie categorie) => await _repo.AddAsync(categorie);
    public async Task UpdateCategorie(Categorie categorie) => await _repo.UpdateAsync(categorie);
    public async Task StergeCategorie(int id) => await _repo.DeleteAsync(id);
}
