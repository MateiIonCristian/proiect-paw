/*
 * DESCRIERE:
 * Model de vizualizare (ViewModel) utilizat la înregistrarea unui nou cont de utilizator.
 * Colectează datele de bază (Email, Parolă, Confirmare Parolă) și fișierul opțional pentru poza de profil, incluzând reguli stricte de validare.
 */

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace recenzi_pentru_firme.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Email invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [StringLength(100, ErrorMessage = "{0} trebuie să aibă cel puțin {2} și maxim {1} caractere.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmare Parolă")]
        [Compare("Password", ErrorMessage = "Parola și confirmarea parolei nu se potrivesc.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Poză de profil")]
        public IFormFile? ProfilePictureFile { get; set; }
    }
}
