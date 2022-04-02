using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tickets.Data;
using Tickets.Domain.Entities;
using Tickets.Repository;
using Tickets.Repository.Interfaces;
using Tickets.Services;
using Tickets.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(Program));

//Dependency Injection classes
builder.Services.AddTransient<Iservices<Wedstrijd>, WedstrijdServices>();
builder.Services.AddTransient<IDAO<Club>, ClubsDAO>();

builder.Services.AddTransient<Iservices<Club>, ClubServices>();
builder.Services.AddTransient<IDAO<Wedstrijd>, WedstrijdDAO>();

builder.Services.AddTransient<Iservices<VakStadion>, VakStadionServices>();
builder.Services.AddTransient<IDAO<VakStadion>, VakStadionDAO>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
