using Microsoft.EntityFrameworkCore;
using SistemaPizzariaDemo.Data;
using SistemaPizzariaDemo.Options;
using SistemaPizzariaDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection não foi configurada.");

builder.Services.AddDbContext<PizzariaDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

builder.Services.Configure<GoogleMapsOptions>(builder.Configuration.GetSection("GoogleMaps"));
builder.Services.AddHttpClient<IGoogleMapsGeocodingService, GoogleMapsGeocodingService>();
builder.Services.AddScoped<IConfiguracaoService, ConfiguracaoService>();
builder.Services.AddScoped<IMotoboyService, MotoboyService>();
builder.Services.AddScoped<IEntregaAgrupamentoService, EntregaAgrupamentoService>();
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
