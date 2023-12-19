using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StocksAppAssignment.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IStocksService _stocksService;

        public StocksController(IOptions<TradingOptions> options, IStocksService stocksService)
        {
            _tradingOptions = options.Value;
            _stocksService = stocksService;
        }


        public IActionResult Explore()
        {
            return View();
        }
    }
}
