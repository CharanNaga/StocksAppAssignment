using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using StocksAppAssignment.Controllers;
using StocksAppAssignment.Models;

namespace StocksAppAssignment.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<CreateOrderActionFilter> _logger;

        public CreateOrderActionFilter(ILogger<CreateOrderActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before logic
            _logger.LogInformation("{FilterName}.{MethodName} before execution",
                nameof(CreateOrderActionFilter),nameof(OnActionExecutionAsync));

            if (context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;

                if (orderRequest != null)
                {
                    //update Order date
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;

                    //re-validate model object
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);

                    //If Model state is not valid
                    if (!tradeController.ModelState.IsValid)
                    {
                        List<string> errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        tradeController.ViewBag.Errors = errors;
                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, Quantity = orderRequest.Quantity, StockSymbol = orderRequest.StockSymbol };
                        context.Result = tradeController.View("Index", stockTrade);
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }

            //after logic
            _logger.LogInformation("{FilterName}.{MethodName} after execution",
                nameof(CreateOrderActionFilter), nameof(OnActionExecutionAsync));
        }
    }
}
