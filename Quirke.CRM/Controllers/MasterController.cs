using Microsoft.AspNetCore.Mvc;

namespace Quirke.CRM.Controllers
{
    public class MasterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
