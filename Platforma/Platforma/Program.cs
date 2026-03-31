using Microsoft.EntityFrameworkCore;
using Platforma.Data;
using Platforma.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurare Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- SEED DATA (REPARAT PENTRU SALVARE PERMANENTA) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    // CREĂM BAZA DOAR DACĂ NU EXISTĂ (NU MAI ȘTERGEM NIMIC!)
    context.Database.EnsureCreated();

    // Adăugăm date de test DOAR dacă baza este complet goală
    if (!context.Categorii.Any())
    {
        var cat1 = new Categorie { Nume = "IT & Software" };
        var cat2 = new Categorie { Nume = "Transport" };
        var cat3 = new Categorie { Nume = "Horeca" };
        context.Categorii.AddRange(cat1, cat2, cat3);

        var oras1 = new Oras { Nume = "București" };
        var oras2 = new Oras { Nume = "Cluj-Napoca" };
        context.Orase.AddRange(oras1, oras2);
        
        context.SaveChanges();

        var f1 = new Firma { Nume = "TechSolutions SRL", CategorieId = cat1.Id, OrasId = oras1.Id, Descriere = "Lideri în inovație software.", Adresa = "Str. Viitorului 10" };
        var f2 = new Firma { Nume = "Fast Delivery", CategorieId = cat2.Id, OrasId = oras2.Id, Descriere = "Livrare rapidă.", Adresa = "Bd. Unirii 5" };
        context.Firme.AddRange(f1, f2);
        context.SaveChanges();
    }
}
// -----------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();