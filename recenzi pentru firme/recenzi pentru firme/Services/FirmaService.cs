/*
 * DESCRIERE:
 * Serviciu care implementează logica de business pentru operațiile asociate firmelor,
 * incluzând căutarea firmelor după nume/adresă, regăsirea datelor relaționate (categorie, oraș, recenzii)
 * și aplicarea validărilor specifice la adăugare și actualizare.
 */

using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace recenzi_pentru_firme.Services;

public class FirmaService
{
    private readonly IRepository<Firma> _repo;

    public FirmaService(IRepository<Firma> repo) => _repo = repo;

    public async Task<IEnumerable<Firma>> GetToateFirmele() => 
        await _repo.GetAllAsync("Categorie", "Oras");
    
    public async Task<Firma?> GetFirmaById(int id) => 
        await _repo.GetByIdAsync(id, "Categorie", "Oras", "Servicii", "Recenzii", "Contact");

    public async Task<IEnumerable<Firma>> CautaFirme(string term)
    {
        if (string.IsNullOrEmpty(term)) return await GetToateFirmele();
        return await _repo.SearchAsync(f => f.Nume.Contains(term) || f.Adresa.Contains(term));
    }

    public async Task AdaugaFirma(Firma firma) {
        if (string.IsNullOrEmpty(firma.Nume)) throw new Exception("Numele firmei este obligatoriu.");
        await _repo.AddAsync(firma);
    }

    public async Task UpdateFirma(Firma firma) => await _repo.UpdateAsync(firma);

    public async Task StergeFirma(int id) => await _repo.DeleteAsync(id);

    // Adaugam o metoda pentru a gasi o firma fara include-uri (pentru Edit post)
    public async Task<Firma?> GetFirmaSimpla(int id) => await _repo.GetByIdAsync(id);
}
