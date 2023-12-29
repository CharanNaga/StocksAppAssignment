﻿using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubService:IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubService> _logger;
        public FinnhubService(IFinnhubRepository finnhubRepository,ILogger<FinnhubService> logger)
        {
           _finnhubRepository = finnhubRepository;
           _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            try
            {
                //writing log message
                _logger.LogInformation($"In {nameof(FinnhubService)}.{nameof(GetCompanyProfile)} Service method");
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
                return responseDictionary;
            }
            catch (Exception ex)
            {
                throw new FinnhubException("Unable to connect to finnhub ",ex);
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            try
            {
                //writing log message
                _logger.LogInformation($"In {nameof(FinnhubService)}.{nameof(GetStockPriceQuote)} Service method");
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
                return responseDictionary;
            }
            catch(Exception ex)
            {
                throw new FinnhubException("Unable to connect to finnhub ", ex);
            }
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            try
            {
                List<Dictionary<string, string>>? responseDictionaryList = await _finnhubRepository.GetStocks();
                return responseDictionaryList;
            }
            catch(Exception ex)
            {
                throw new FinnhubException("Unable to connect to finnhub", ex);
            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            try
            {
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
                return responseDictionary;
            }
            catch(Exception ex)
            {
                throw new FinnhubException("Unable to connect to finnhub ", ex);
            }
        }
    }
}