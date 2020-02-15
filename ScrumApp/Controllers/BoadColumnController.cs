using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScrumApp.Controllers
{
    public class BoadColumnController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}