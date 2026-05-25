/*
 * DESCRIERE:
 * Definițiile modelelor de date principale ale aplicației (Categorie, Oras, Firma, Recenzie, Serviciu și Contact).
 * Aceste clase stabilesc structura bazei de date și relațiile dintre entități, utilizând adnotări de validare
 * pentru a asigura integritatea datelor atât la nivel de cod, cât și în baza de date și în interfața grafică.
 */

using System.ComponentModel.DataAnnotations;

namespace recenzi_pentru_firme.Models;

public class Categorie
{
    public int Id { get; set; }
    [Required, StringLength(50)]
    public string Nume { get; set; } = string.Empty;
    public virtual ICollection<Firma> Firme { get; set; } = new List<Firma>();
}

public class Oras
{
    public int Id { get; set; }
    [Required, StringLength(50)]
    public string Nume { get; set; } = string.Empty;
    public virtual ICollection<Firma> Firme { get; set; } = new List<Firma>();
}

public class Firma
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Nume { get; set; } = string.Empty;
    public string? Descriere { get; set; }
    public string? Adresa { get; set; }
    
    public int CategorieId { get; set; }
    public virtual Categorie? Categorie { get; set; }
    
    public int OrasId { get; set; }
    public virtual Oras? Oras { get; set; }

    public virtual Contact? Contact { get; set; }
    public virtual ICollection<Recenzie> Recenzii { get; set; } = new List<Recenzie>();
    public virtual ICollection<Serviciu> Servicii { get; set; } = new List<Serviciu>();
}

public class Recenzie
{
    public int Id { get; set; }
    [Required]
    public string Autor { get; set; } = string.Empty;
    [Required, Range(1, 5)]
    public int Nota { get; set; }
    [Required]
    public string Continut { get; set; } = string.Empty;
    
    public int FirmaId { get; set; }
    public virtual Firma? Firma { get; set; }
}

public class Serviciu
{
    public int Id { get; set; }
    [Required]
    public string Denumire { get; set; } = string.Empty;
    public int FirmaId { get; set; }
    public virtual Firma? Firma { get; set; }
}

public class Contact
{
    [Key]
    public int FirmaId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public virtual Firma? Firma { get; set; }
}
