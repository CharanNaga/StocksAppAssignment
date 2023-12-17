using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly StockMarketDbContext _db;

        public StocksService(StockMarketDbContext db)
        {
            _db = db;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            //1. Check buyOrderRequest != null
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            //2. Validate all properties of buyOrderRequest
            ValidationHelper.ModelValidation(buyOrderRequest); //validating all properties using Model validations by calling a reusable method.

            //3. Convert BuyOrderRequest to BuyOrder type
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //4. Generate a new BuyOrderID
            buyOrder.BuyOrderID = Guid.NewGuid();

            //5. Then add it to the List<BuyOrder>
            //_buyOrders.Add(buyOrder);
            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();

            //6. Return BuyOrderResponse object with generated BuyOrderID.
            return buyOrder.ToBuyOrderResponse();
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
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();

            //6. Return SellOrderResponse object with generated SellOrderID.
            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            //Converts all buyorders from "BuyOrder" type to "BuyOrderResponse" type.
            //Return all BuyOrderResponse Objects.
            //return Task.FromResult(_buyOrders.Select(b => b.ToBuyOrderResponse()).ToList());

            var buyOrders = await _db.BuyOrders.OrderByDescending(b=>b.DateAndTimeOfOrder).ToListAsync();
            return buyOrders.Select(b => b.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            //Converts all sellorders from "SellOrder" type to "SellOrderResponse" type.
            //Return all SellOrderResponse Objects.
            //return _sellOrders.Select(s => s.ToSellOrderResponse()).ToList();

            var sellOrders = await _db.SellOrders.OrderByDescending(s => s.DateAndTimeOfOrder).ToListAsync();
            return sellOrders.Select(s => s.ToSellOrderResponse()).ToList();
        }
    }
}
