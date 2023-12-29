using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StocksAppAssignment.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;

        private readonly IConfiguration _configuration;

        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions,IFinnhubCompanyProfileService finnhubCompanyProfileService, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService,IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDictionary = null;

            if (stockSymbol != null)
            {
                companyProfileDictionary = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
                var stockPriceDictionary = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);
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