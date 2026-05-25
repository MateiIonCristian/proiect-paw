/*
 * DESCRIERE:
 * Modelul utilizatorului aplicației, care extinde clasa IdentityUser din ASP.NET Core Identity.
 * Permite stocarea unei poze de profil personalizate sub formă de tablou de octeți (byte array) direct în baza de date.
 */

using Microsoft.AspNetCore.Identity;

namespace recenzi_pentru_firme.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Poza de profil stocată ca byte array în baza de date
        public byte[]? ProfilePicture { get; set; }
    }
}
