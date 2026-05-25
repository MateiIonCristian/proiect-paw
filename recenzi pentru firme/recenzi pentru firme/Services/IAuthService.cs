/*
  --------------------------------------------------------------------------------
  DESCRIERE: Interfata pentru serviciul de autentificare. Definese operatiunile
             de inregistrare, logare, delogare si management al profilului utilizatorului,
             asigurand decuplarea completa a logicii de business de controller.
  --------------------------------------------------------------------------------
*/
using Microsoft.AspNetCore.Identity;
using recenzi_pentru_firme.Models.ViewModels;
using System.Security.Claims;

namespace recenzi_pentru_firme.Services
{
    public interface IAuthService
    {
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<EditProfileViewModel?> GetEditProfileViewModelAsync(ClaimsPrincipal userPrincipal);
        Task<IdentityResult> UpdateProfileAsync(ClaimsPrincipal userPrincipal, EditProfileViewModel model);
        Task LogoutAsync();
    }
}
