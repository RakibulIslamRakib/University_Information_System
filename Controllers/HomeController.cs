using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using University_Information_System.Models;

namespace University_Information_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 

    }
}