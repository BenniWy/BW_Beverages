using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BW_Beverages.Controllers
{
    public class AboutController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}