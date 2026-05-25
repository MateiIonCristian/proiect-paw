/*
  --------------------------------------------------------------------------------
  DESCRIERE: Controller-ul pentru conturile de utilizator. Este complet decuplat,
             fara dependinte directe de baze de date sau UserManager. Delega toate 
             operatiunile de business catre IAuthService si se ocupa exclusiv de rutare, 
             redirectionari si gestiunea starii formularelor (ModelState).
Gestionează conturile de utilizatori (Login, Register, Log out, vizualizarea și editarea
profilului/pozei de profil).
  --------------------------------------------------------------------------------
*/
using Microsoft.AspNetCore.Mvc;
using recenzi_pentru_firme.Models.ViewModels;
using recenzi_pentru_firme.Services;
using Microsoft.AspNetCore.Authorization;

namespace recenzi_pentru_firme.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    if (error.Code != null && error.Code.StartsWith("ProfilePicture"))
                    {
                        ModelState.AddModelError("ProfilePictureFile", error.Description);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(model);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Contul este blocat temporar din cauza incercarilor esuate repetate.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Autentificarea nu este permisa pentru acest cont (ex: e-mail neconfirmat).");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Adresa de e-mail sau parola incorecta.");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfil()
        {
            var model = await _authService.GetEditProfileViewModelAsync(User);
            if (model == null) return Challenge();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfil(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.UpdateProfileAsync(User, model);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Profilul a fost actualizat cu succes!";
                    return RedirectToAction("Profil", "Home");
                }

                foreach (var error in result.Errors)
                {
                    if (error.Code != null && error.Code.StartsWith("NewProfilePicture"))
                    {
                        ModelState.AddModelError("NewProfilePictureFile", error.Description);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Daca validarea a esuat, reincarcam poza curenta pentru a o afisa in formular
            var currentModel = await _authService.GetEditProfileViewModelAsync(User);
            if (currentModel != null)
            {
                model.CurrentProfilePicture = currentModel.CurrentProfilePicture;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
