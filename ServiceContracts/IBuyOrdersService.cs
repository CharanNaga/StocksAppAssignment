using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents Stocks service that includes operations like buy order and get buy orders
    /// </summary>
    public interface IBuyOrdersService
    {
        /// <summary>
        /// Creates a buy order
        /// </summary>
        /// <param name="buyOrderRequest">Buy order object</param>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        /// <summary>
        /// Returns all existing buy orders
        /// </summary>
        /// <returns>Returns a list of objects of BuyOrder type</returns>
        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
