using System.ComponentModel.DataAnnotations;

namespace Platforma.Models
{
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
        
        public int? CategorieId { get; set; }
        public virtual Categorie? Categorie { get; set; }
        
        public int? OrasId { get; set; }
        public virtual Oras? Oras { get; set; }
        
        public virtual ICollection<Recenzie> Recenzii { get; set; } = new List<Recenzie>();
        public virtual ICollection<Serviciu> Servicii { get; set; } = new List<Serviciu>();
    }

    public class Recenzie
    {
        public int Id { get; set; }
        [Required, Range(1, 5)]
        public int Nota { get; set; }
        [Required]
        public string Comentariu { get; set; } = string.Empty;
        public DateTime DataPublicarii { get; set; } = DateTime.Now;
        
        public int FirmaId { get; set; }
        public virtual Firma? Firma { get; set; }
        
        public virtual ICollection<RaspunsRecenzie> Raspunsuri { get; set; } = new List<RaspunsRecenzie>();
    }

    public class Serviciu
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Denumire { get; set; } = string.Empty;
        public int FirmaId { get; set; }
        public virtual Firma? Firma { get; set; }
    }

    public class RaspunsRecenzie
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
        public int RecenzieId { get; set; }
        public virtual Recenzie? Recenzie { get; set; }
    }
}
