
using DocMeetingPro.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie(opts =>
     {
         opts.Cookie.Name = ".DocMeetingPro.auth";
         //kodda bir deðiþiklik yapýldýðýnda bunun kullanýcýya yansýmasý için süre 1 ay 1 yýl gibi çok uzun tutulmamalý
         opts.ExpireTimeSpan = TimeSpan.FromDays(7);
         opts.SlidingExpiration = false;
         opts.LoginPath = "/Account/Login";
         opts.LogoutPath = "/Account/Logout";
         opts.AccessDeniedPath = "/Home/AccessDenied";
     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
