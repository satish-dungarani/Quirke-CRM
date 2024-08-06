using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CurrentStock()
        {
            return View();
        }
        public IActionResult History()
        {
            return View();
        }
    }
}
