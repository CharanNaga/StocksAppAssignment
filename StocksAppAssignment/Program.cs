using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using ServiceContracts;
using Services;
using StocksAppAssignment;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();

builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpClient();

var app = builder.Build();

RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
