using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StocksService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }
        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
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
            _buyOrders.Add(buyOrder);

            //6. Return PersonResponse object with generated PersonID.
            return Task.FromResult(buyOrder.ToBuyOrderResponse());
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            throw new NotImplementedException();
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            throw new NotImplementedException();
        }
    }
}
