using ccalendar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ccalendar.Services.Interfaces;
using ccalendar.Services;

var builder = WebApplication.CreateBuilder(args);

// Aggiunge il database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Aggiunge Identity
// builder.Services.AddDefaultIdentity<IdentityUser>(options =>
// {
//     options.SignIn.RequireConfirmedAccount = true;
//     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); 
//     options.Lockout.MaxFailedAccessAttempts = 5;
//     options.Lockout.AllowedForNewUsers = true;
// }) 
// .AddEntityFrameworkStores<ApplicationDbContext>()
// .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddRazorPages(options =>
// {
//     options.Conventions.AuthorizeAreaFolder("Identity", "/");
//     options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login");
//     options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Logout");
// });

// Dependency Injection
builder.Services.AddScoped<IHomeServices, HomeServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.MapRazorPages();

// app.UseHttpsRedirection();
app.UseRouting();

// app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
