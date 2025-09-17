var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddServices(builder.Configuration)
    .AddJwtBearerAuth();

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
