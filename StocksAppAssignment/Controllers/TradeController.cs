using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksAppAssignment.Filters.ActionFilters;
using StocksAppAssignment.Models;

namespace StocksAppAssignment.Controllers
{
    [Route("[controller]")] //Route Token
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IBuyOrdersService _buyOrdersService;
        private readonly ISellOrdersService _sellOrdersService;

        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;

        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="finnhubCompanyProfileService">Injecting FinnhubService</param>
        /// <param name="buyOrdersService">Injecting StocksService</param>
        /// <param name="configuration">Injecting IConfiguration</param>
        /// <param name="logger">Injecting ILogger</param>

        public TradeController(IOptions<TradingOptions> tradingOptions, IBuyOrdersService buyOrdersService,ISellOrdersService sellOrdersService, IFinnhubCompanyProfileService finnhubCompanyProfileService, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService,IConfiguration configuration, ILogger<TradeController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _buyOrdersService = buyOrdersService;
            _sellOrdersService = sellOrdersService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
            _configuration = configuration;
            _logger = logger;
        }

        [Route("[action]/{stockSymbol}")]
        [Route("~/[controller]/{stockSymbol}")]
        [HttpGet]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            //writing logging messages
            _logger.LogInformation($"In {nameof(TradeController)}.{nameof(Index)} action method");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");

            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";

            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

            //create model object
            StockTrade stockTrade = new StockTrade() { StockSymbol = stockSymbol };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() { StockSymbol = companyProfileDictionary["ticker"].ToString(), StockName = companyProfileDictionary["name"].ToString(), Quantity = _tradingOptions.DefaultOrderQuantity ?? 0, Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }
        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest) 
        {
            BuyOrderResponse buyOrderResponse = await _buyOrdersService.CreateBuyOrder(orderRequest);
            return RedirectToAction("Orders","Trade");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            SellOrderResponse buyOrderResponse = await _sellOrdersService.CreateSellOrder(orderRequest);
            return RedirectToAction("Orders", "Trade");
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponse = await _buyOrdersService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponse = await _sellOrdersService.GetSellOrders();
            Orders orders = new Orders()
            {
                BuyOrders = buyOrderResponse,
                SellOrders = sellOrderResponse
            };
            ViewBag.TradingOptions = _tradingOptions;
            return View(orders);
        }
        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            //Get list of orders
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await _buyOrdersService.GetBuyOrders());
            orders.AddRange(await _sellOrdersService.GetSellOrders());
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradingOptions;
            //Return view as pdf
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Orientation.Landscape
            };
        }
    }
}