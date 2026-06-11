using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Data;
using StudyBuddyApp.Data.Seed;
using StudyBuddyApp.Models;
using StudyBuddyApp.Services;
using StudyBuddyApp.Services.Notifications;
using StudyBuddyApp.Services.Observers;
using StudyBuddyApp.Services.Sessions;
using StudyBuddyApp.Services.Statistics;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Korisnik>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequiredLength = 7;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ObavjestenjeFactory>();
builder.Services.AddScoped<EksterniEmailServis>();
builder.Services.AddScoped<IObavjestenjeSender, EmailSenderAdapter>();
builder.Services.AddScoped<ISesijaObserver, SesijaObavjestenjeObserver>();
builder.Services.AddScoped<SesijaSubject>();
builder.Services.AddScoped<SesijaFacade>();
builder.Services.AddScoped<IStatistikaStrategy, BrojSesijaStrategy>();
builder.Services.AddScoped<IStatistikaStrategy, BrojPrisustavaStrategy>();
builder.Services.AddScoped<IStatistikaStrategy, UkupnoVrijemeUcenjaStrategy>();
builder.Services.AddScoped<StatistikaContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    db.Database.Migrate();

    await PredmetSeeder.SeedAsync(db);
    await LokacijaSeeder.SeedAsync(db);
}

app.Run();