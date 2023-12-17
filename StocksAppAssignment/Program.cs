using Entities;
using Microsoft.EntityFrameworkCore;
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
    options.UseSqlServer();
});

builder.Services.AddHttpClient();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
