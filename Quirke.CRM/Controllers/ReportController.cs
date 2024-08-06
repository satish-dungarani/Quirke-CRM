using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Customer()
        {
            return View();
        }
        public IActionResult EmployeLeave()
        {
            return View();
        }
        public IActionResult Compliance()
        {
            return View();
        }
        public IActionResult Stocks()
        {
            return View();
        }
    }
}
