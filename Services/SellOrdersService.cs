using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;

namespace Services
{
    public class SellOrdersService : ISellOrdersService
    {
        private readonly IStocksRepository _stocksRepository;

        public SellOrdersService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            //1. Check sellOrderRequest != null
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            //2. Validate all properties of sellOrderRequest
            ValidationHelper.ModelValidation(sellOrderRequest); //validating all properties using Model validations by calling a reusable method.

            //3. Convert SellOrderRequest to SellOrder type
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            //4. Generate a new SellOrderID
            sellOrder.SellOrderID = Guid.NewGuid();

            //5. Then add it to the List<SellOrder>
            //_sellOrders.Add(sellOrder);
            await _stocksRepository.CreateSellOrder(sellOrder);

            //6. Return SellOrderResponse object with generated SellOrderID.
            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            //Converts all sellorders from "SellOrder" type to "SellOrderResponse" type.
            //Return all SellOrderResponse Objects.
            //return _sellOrders.Select(s => s.ToSellOrderResponse()).ToList();

            var sellOrders = await _stocksRepository.GetSellOrders();
            return sellOrders.Select(s => s.ToSellOrderResponse()).ToList();
        }
    }
}
