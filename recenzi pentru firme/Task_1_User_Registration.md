# Task 1: Create User Registration Functionality

Acest fișier conține toate componentele necesare pentru implementarea funcționalității de **Înregistrare Utilizator (User Registration)** din cadrul aplicației tale. Codul este separat pe straturi conform arhitecturii MVC din ASP.NET Core și conține validări complete atât pe client, cât și pe server, inclusiv suport pentru stocarea imaginii de profil în baza de date ca `byte[]`.

---

## 1. Modelul de Vizualizare (ViewModel)
**Calea fișierului:** `Models/ViewModels/RegisterViewModel.cs`

Acest model capturează datele introduse de utilizator în formularul de înregistrare și realizează validările de bază (câmpuri obligatorii, format de e-mail valid, lungime minimă parolă, potrivire parole și încărcare fișier de tip imagine).

```csharp
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
```

---

## 2. Serviciul de Autentificare & Înregistrare (Interfață & Implementare)
Pentru a menține controllerul curat și a respecta bunele practici, logica de business este extrasă într-un serviciu separat.

### A. Interfața Serviciului
**Calea fișierului:** `Services/IAuthService.cs`

```csharp
using Microsoft.AspNetCore.Identity;
using recenzi_pentru_firme.Models.ViewModels;

namespace recenzi_pentru_firme.Services
{
    public interface IAuthService
    {
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task LogoutAsync();
    }
}
```

### B. Implementarea Serviciului
**Calea fișierului:** `Services/AuthService.cs`

Serviciul convertește poza de profil încărcată ca `IFormFile` într-un tablou de octeți (`byte[]`), creează noul cont folosind clasa `UserManager`, îi atribuie în mod automat rolul implicit de `"User"` și îl autentifică automat în aplicație.

```csharp
using Microsoft.AspNetCore.Identity;
using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Models.ViewModels;

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

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
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
                // Atribuirea rolului implicit de User
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
```

---

## 3. Controllerul de Autentificare (Controller)
**Calea fișierului:** `Controllers/AccountController.cs`

Controllerul expune acțiunile HTTP GET (pentru afișarea formularului) și HTTP POST (pentru procesarea trimiterii datelor). Apelează `IAuthService` pentru efectuarea înregistrării și gestionează erorile apărute.

```csharp
using Microsoft.AspNetCore.Mvc;
using recenzi_pentru_firme.Models.ViewModels;
using recenzi_pentru_firme.Services;

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
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
```

---

## 4. Pagina de Înregistrare (Razor View)
**Calea fișierului:** `Views/Account/Register.cshtml`

Interfața grafică modernă realizată cu stiluri personalizate premium (gradient elegant de fundal, margini rotunjite și umbre moderne), tag helper-e ASP.NET Core pentru legarea bidirecțională a datelor și validare dinamică pe parte de client (`_ValidationScriptsPartial`).

```html
@model recenzi_pentru_firme.Models.ViewModels.RegisterViewModel

@{
    ViewData["Title"] = "Înregistrare";
}

<div class="row justify-content-center align-items-center mt-5 mb-5">
    <div class="col-md-8 col-lg-6 animate-fade-up">
        <div class="card card-premium shadow-lg border-0 overflow-hidden">
            <div class="py-5 text-center text-white" style="background: linear-gradient(135deg, #10b981, #059669);">
                <i class="fas fa-user-plus fa-4x mb-3"></i>
                <h3 class="fw-bold mb-0">Creează Cont Nou</h3>
                <p class="small opacity-75 mt-2">Alătură-te comunității noastre astăzi!</p>
            </div>
            <div class="card-body p-4 p-lg-5">
                <form asp-controller="Account" asp-action="Register" method="post" enctype="multipart/form-data">
                    <div asp-validation-summary="ModelOnly" class="text-danger mb-3"></div>

                    <div class="mb-4">
                        <label asp-for="Email" class="form-label fw-bold">Adresă de Email</label>
                        <input asp-for="Email" class="form-control form-control-premium" placeholder="nume@exemplu.com" />
                        <span asp-validation-for="Email" class="text-danger"></span>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-6">
                            <label asp-for="Password" class="form-label fw-bold">Parolă</label>
                            <input asp-for="Password" type="password" class="form-control form-control-premium" placeholder="••••••••" />
                            <span asp-validation-for="Password" class="text-danger"></span>
                        </div>
                        <div class="col-md-6">
                            <label asp-for="ConfirmPassword" class="form-label fw-bold">Confirmare Parolă</label>
                            <input asp-for="ConfirmPassword" type="password" class="form-control form-control-premium" placeholder="••••••••" />
                            <span asp-validation-for="ConfirmPassword" class="text-danger"></span>
                        </div>
                    </div>

                    <div class="mb-4">
                        <label asp-for="ProfilePictureFile" class="form-label fw-bold"><i class="fas fa-camera me-1"></i> Poză de profil (Opțional)</label>
                        <input asp-for="ProfilePictureFile" class="form-control form-control-premium" type="file" accept="image/*" />
                        <span asp-validation-for="ProfilePictureFile" class="text-danger"></span>
                    </div>

                    <div class="mt-4 d-grid">
                        <button type="submit" class="btn btn-premium btn-lg" style="background: linear-gradient(135deg, #10b981, #059669);">Creează Cont</button>
                    </div>
                </form>
            </div>
            <div class="card-footer text-center py-4 bg-light border-0">
                <div class="small text-muted">Ai deja un cont? <a asp-action="Login" class="text-success fw-bold text-decoration-none">Autentifică-te!</a></div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```
