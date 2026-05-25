/*
  --------------------------------------------------------------------------------
  DESCRIERE: Serviciul dedicat pentru logica de autentificare si gestionare a utilizatorilor.
             - Gestioneaza crearea si logarea conturilor cu ASP.NET Core Identity.
             - Contine validarile de securitate pe server pentru pozele de profil (max 2MB, tip MIME).
             - Reimprospateaza cookie-urile de sesiune instant prin RefreshSignInAsync.
  --------------------------------------------------------------------------------
*/
using Microsoft.AspNetCore.Identity;
using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Models.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace recenzi_pentru_firme.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var errors = new List<IdentityError>();

            // 1. Validari imagine profil (daca este furnizata)
            if (model.ProfilePictureFile != null)
            {
                if (model.ProfilePictureFile.Length > 2 * 1024 * 1024)
                {
                    errors.Add(new IdentityError { Code = "ProfilePictureSize", Description = "Poza de profil nu poate depasi 2 MB." });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(model.ProfilePictureFile.FileName)?.ToLower();
                if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                {
                    errors.Add(new IdentityError { Code = "ProfilePictureExtension", Description = "Sunt permise doar imagini (.jpg, .jpeg, .png, .gif)." });
                }

                if (!model.ProfilePictureFile.ContentType.StartsWith("image/"))
                {
                    errors.Add(new IdentityError { Code = "ProfilePictureMime", Description = "Fisierul incarcat trebuie sa fie o imagine valida." });
                }
            }

            if (errors.Any())
            {
                return IdentityResult.Failed(errors.ToArray());
            }

            byte[]? profilePictureBytes = null;
            if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ProfilePictureFile.CopyToAsync(memoryStream);
                    profilePictureBytes = memoryStream.ToArray();
                }
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                ProfilePicture = profilePictureBytes
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task<EditProfileViewModel?> GetEditProfileViewModelAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null) return null;

            return new EditProfileViewModel
            {
                Email = user.Email,
                CurrentProfilePicture = user.ProfilePicture
            };
        }

        public async Task<IdentityResult> UpdateProfileAsync(ClaimsPrincipal userPrincipal, EditProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Utilizatorul nu a fost gasit." });
            }

            var errors = new List<IdentityError>();

            // 1. Validari imagine profil noua
            if (model.NewProfilePictureFile != null)
            {
                if (model.NewProfilePictureFile.Length > 2 * 1024 * 1024)
                {
                    errors.Add(new IdentityError { Code = "NewProfilePictureSize", Description = "Poza de profil nu poate depasi 2 MB." });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(model.NewProfilePictureFile.FileName)?.ToLower();
                if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                {
                    errors.Add(new IdentityError { Code = "NewProfilePictureExtension", Description = "Sunt permise doar imagini (.jpg, .jpeg, .png, .gif)." });
                }

                if (!model.NewProfilePictureFile.ContentType.StartsWith("image/"))
                {
                    errors.Add(new IdentityError { Code = "NewProfilePictureMime", Description = "Fisierul incarcat trebuie sa fie o imagine valida." });
                }
            }

            if (errors.Any())
            {
                return IdentityResult.Failed(errors.ToArray());
            }

            if (model.RemoveCurrentPicture)
            {
                user.ProfilePicture = null;
            }
            else if (model.NewProfilePictureFile != null && model.NewProfilePictureFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.NewProfilePictureFile.CopyToAsync(memoryStream);
                    user.ProfilePicture = memoryStream.ToArray();
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
            }
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
