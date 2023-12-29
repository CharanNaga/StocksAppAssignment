using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StocksAppAssignment.Models;

namespace StocksAppAssignment.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>(); 
            if(exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            {
                //ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
                Error error = new Error() { ErrorMessage = exceptionHandlerPathFeature.Error.Message};
                return View(error);
            }
            else
            {
                //ViewBag.ErrorMessage = "Error Occured";
                Error error = new Error() { ErrorMessage = "Error Occured" };
                return View(error);
            }
           
        }
    }
}
