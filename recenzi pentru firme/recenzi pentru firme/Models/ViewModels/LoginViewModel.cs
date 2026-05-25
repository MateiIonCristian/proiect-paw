/*
 * DESCRIERE:
 * Model de vizualizare (ViewModel) destinat colectării și validării credențialelor de autentificare (Email și Parolă)
 * introduse de un utilizator în cadrul formularului de Login.
 */

using System.ComponentModel.DataAnnotations;

namespace recenzi_pentru_firme.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Email invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Ține-mă minte")]
        public bool RememberMe { get; set; }
    }
}
