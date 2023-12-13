﻿using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksAppAssignment.Models;

namespace StocksAppAssignment.Controllers
{
    [Route("[controller]")] //Route Token
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly IStocksService _stocksService;
        private readonly TradingOptions _tradingOptions;

        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="finnhubService">Injecting FinnhubService</param>
        /// <param name="stocksService">Injecting StocksService</param>
        /// <param name="configuration">Injecting IConfiguration</param>

        public TradeController(IFinnhubService finnhubService, IConfiguration configuration,IStocksService stocksService,IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
            _stocksService = stocksService;
            _tradingOptions = tradingOptions.Value;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("~/[controller]")]
        public async Task<IActionResult> Index()
        {
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";

            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);


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
                    Quantity = _tradingOptions.DefaultOrderQuantity ?? 0,
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString())
                };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
            return View(stockTrade);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest) 
        {
            //update Order date
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //re-validate model object
            ModelState.Clear();
            TryValidateModel(buyOrderRequest);
            
            //If Model state is not valid
            if(!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(v=>v.Errors).Select(e=>e.ErrorMessage).ToList();
                ViewBag.Errors = errors;
                StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction("Orders","Trade");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            //update Order date
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //re-validate model object
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            //If Model state is not valid
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.Errors = errors;
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }
            SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction("Orders", "Trade");
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponse = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponse = await _stocksService.GetSellOrders();
            Orders orders = new Orders()
            {
                BuyOrders = buyOrderResponse,
                SellOrders = sellOrderResponse
            };
            ViewBag.TradingOptions = _tradingOptions;
            return View(orders);
        }
    }
}