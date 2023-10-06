using Microsoft.AspNetCore.Mvc;

namespace StocksAppAssignment.Controllers
{
    public class TradeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
