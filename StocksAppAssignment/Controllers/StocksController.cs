using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories;
using ServiceContracts;
using StocksAppAssignment.Models;

namespace StocksAppAssignment.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubStocksService _finnhubStocksService;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IOptions<TradingOptions> options,IFinnhubStocksService finnhubStocksService, ILogger<StocksController> logger)
        {
            _tradingOptions = options.Value;
            _finnhubStocksService = finnhubStocksService;
            _logger = logger;
        }

        [Route("/")]
        [Route("[action]/{stock?}")]
        [Route("~/[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock,bool showAll = false)
        {
            //writing logging message
            _logger.LogInformation($"In {nameof(StocksController)}.{nameof(Explore)} action method");

            //get company profile from Finnhub API server
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubStocksService.GetStocks();

            List<Stock> stocks = new List<Stock>();

            if (stocksDictionary != null)
            {
                //filter stocks
                if (!showAll && _tradingOptions.Top25PopularStocks != null)
                {
                    string[]? top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");
                    if (top25PopularStocksList != null)
                    {
                        stocksDictionary = stocksDictionary
                         .Where(temp => top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
                         .ToList();
                    }
                }
                //convert dictionary objects into Stock objects
                stocks = stocksDictionary
                 .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                .ToList();
            }
            ViewBag.stock = stock;
            return View(stocks);
        }
    }
}