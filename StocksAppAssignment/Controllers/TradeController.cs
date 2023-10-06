using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksAppAssignment.Models;

namespace StocksAppAssignment.Controllers
{
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly TradingOptions _tradingOptions;

        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="finnhubService">Injecting FinnhubService</param>
        /// <param name="configuration">Injecting IConfiguration</param>

        public TradeController(IFinnhubService finnhubService, IConfiguration configuration,IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
            _tradingOptions = tradingOptions.Value;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("~/[controller]")]
        public IActionResult Index()
        {
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";

            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);


            //create model object
            StockTrade stockTrade = new StockTrade()
            { 
                StockSymbol = _tradingOptions.DefaultStockSymbol
            };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() 
                { 
                    StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]),
                    StockName = Convert.ToString(companyProfileDictionary["name"]), 
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString())
                };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }
    }
}
