using NextFarma.Data; // Namespace do seu DbContext
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container (Antigo ConfigureServices do Startup.cs)
builder.Services.AddControllersWithViews();

// --- CONFIGURAÇÃO DO MYSQL ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
builder.Services.AddScoped<SeedingService>();
// -----------------------------

var app = builder.Build();

// --- BLOCO DE SEEDING ---
// Isso cria um escopo temporário para pegar o serviço e rodar
using (var scope = app.Services.CreateScope())
{
    var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
    seedingService.Seed();
}

// Configura o pipeline de requisição (Antigo Configure do Startup.cs)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();