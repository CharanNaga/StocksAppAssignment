using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class BuyOrdersService : IBuyOrdersService
    {
        private readonly IStocksRepository _stocksRepository;

        public BuyOrdersService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
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
            await _stocksRepository.CreateBuyOrder(buyOrder);

            //6. Return BuyOrderResponse object with generated BuyOrderID.
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            //Converts all buyorders from "BuyOrder" type to "BuyOrderResponse" type.
            //Return all BuyOrderResponse Objects.
            //return Task.FromResult(_buyOrders.Select(b => b.ToBuyOrderResponse()).ToList());

            var buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(b => b.ToBuyOrderResponse()).ToList();
        }
    }
}
