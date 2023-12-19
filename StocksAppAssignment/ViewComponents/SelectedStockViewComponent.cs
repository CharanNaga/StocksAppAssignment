using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StocksAppAssignment.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IStocksService stocksService, IFinnhubService finnhubService, IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _stocksService = stocksService;
            _finnhubService = finnhubService;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDictionary = null;

            if (stockSymbol != null)
            {
                companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);
                var stockPriceDictionary = await _finnhubService.GetStockPriceQuote(stockSymbol);
                if (stockPriceDictionary != null && companyProfileDictionary != null)
                {
                    companyProfileDictionary.Add("price", stockPriceDictionary["c"]);
                }
            }

            if (companyProfileDictionary != null && companyProfileDictionary.ContainsKey("logo"))
                return View(companyProfileDictionary);
            else
                return Content("");
        }
    }
}