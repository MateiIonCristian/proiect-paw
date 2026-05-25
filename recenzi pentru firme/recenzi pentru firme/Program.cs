/*
  --------------------------------------------------------------------------------
  DESCRIERE: Punctul de pornire (Entry Point) al aplicatiei web ASP.NET Core.
             - Configureaza containerele de Dependency Injection (DbContext, Identity).
             - Inregistreaza repositories si business services (IAuthService).
             - Ruleaza logica de Seeding automata pentru roluri si contul de Administrator.
  --------------------------------------------------------------------------------
*/
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using recenzi_pentru_firme.Models.Repositories;
using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Services;
using recenzi_pentru_firme.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurare Connection String (Default: LocalDB)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=RecenziiFirmeDB;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

// --- INREGISTRARE SERVICII ---

// 1. Repositories
builder.Services.AddScoped<IRepository<Firma>, FirmaRepository>();
builder.Services.AddScoped<IRepository<Recenzie>, RecenzieRepository>();
builder.Services.AddScoped<IRepository<Categorie>, CategorieRepository>();
builder.Services.AddScoped<IRepository<Oras>, OrasRepository>();

// 2. Business Logic Services
builder.Services.AddScoped<FirmaService>();
builder.Services.AddScoped<RecenzieService>();
builder.Services.AddScoped<CategorieService>();
builder.Services.AddScoped<OrasService>();

// 3. Auth Service
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// --- START SEED DATA ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.Categorii.Any())
    {
        context.Categorii.AddRange(
            new Categorie { Nume = "IT & Tehnologie" },
            new Categorie { Nume = "Restaurante & Cafele" },
            new Categorie { Nume = "Servicii Auto" }
        );
    }

    if (!context.Orase.Any())
    {
        context.Orase.AddRange(
            new Oras { Nume = "București" },
            new Oras { Nume = "Cluj-Napoca" },
            new Oras { Nume = "Iași" }
        );
    }
    context.SaveChanges();

    // Identity Seed (Roles & Admin)
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleManager.RoleExistsAsync("Administrator"))
        await roleManager.CreateAsync(new IdentityRole("Administrator"));
    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));

    var adminUser = await userManager.FindByEmailAsync("admin@test.com");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser { UserName = "admin@test.com", Email = "admin@test.com" };
        var result = await userManager.CreateAsync(adminUser, "admin123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
    else
    {
        // Resetăm parola la "admin123" pentru a fi siguri că utilizatorul o poate folosi
        var hasPassword = await userManager.HasPasswordAsync(adminUser);
        if (hasPassword) await userManager.RemovePasswordAsync(adminUser);
        await userManager.AddPasswordAsync(adminUser, "admin123");
    }

    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Administrator"))
    {
        await userManager.AddToRoleAsync(adminUser, "Administrator");
    }
}
// --- END SEED DATA ---

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
