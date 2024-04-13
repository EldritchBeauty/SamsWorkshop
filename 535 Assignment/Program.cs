using _535_Assignment.Models;
using _535_Assignment.Repository;
using _535_Assignment.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ContextShopping>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingDB")));

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<FileUploaderService>();
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddScoped<SanitiserService>();

//Redirected unauthenticated users to the Login page, and sets the time span before a user is automatically signed out.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Home/Login";
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

app.UseSession();


app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "ALLOW-FROM www.youtube.com");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';img-src 'self' data: ; frame-src www.youtube.com; frame-ancestors 'self'; form-action 'self'; script-src-elem 'self' https://cdnjs.cloudflare.com/ajax/libs/");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Strict-Transport-Security", "max-age=63072000; includeSubDomains;");
    await next(context);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
