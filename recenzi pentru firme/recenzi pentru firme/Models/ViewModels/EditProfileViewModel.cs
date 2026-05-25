/*
 * DESCRIERE:
 * Model de vizualizare (ViewModel) utilizat pentru transmiterea datelor necesare editării profilului de utilizator.
 * Gestionează încărcarea unui fișier nou pentru poza de profil, precum și opțiunea de eliminare a pozei curente.
 */

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace recenzi_pentru_firme.Models.ViewModels
{
    public class EditProfileViewModel
    {
        public string? Email { get; set; }

        [Display(Name = "Poză de profil nouă")]
        public IFormFile? NewProfilePictureFile { get; set; }

        [Display(Name = "Șterge poza de profil curentă")]
        public bool RemoveCurrentPicture { get; set; }
        
        public byte[]? CurrentProfilePicture { get; set; }
    }
}
