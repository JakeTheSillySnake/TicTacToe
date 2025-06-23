using Microsoft.EntityFrameworkCore;
using TicTacToe.DataSource.Models;
using TicTacToe.DataSource.Services;
using TicTacToe.Domain.Services;
using TicTacToe.Web.Filters;
using TicTacToe.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Transient, ServiceLifetime.Transient);

builder.Services.AddScoped<IGameDbService, GameDbService>();
builder.Services.AddScoped<IUserDbService, UserDbService>();
builder.Services.AddTransient<AuthFilter>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IGameService, GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Game}/{action=Index}/{id?}");

app.Run();
