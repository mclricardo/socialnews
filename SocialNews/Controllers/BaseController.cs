using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNews.Domain.Model;
using SocialNews.Data;

namespace SocialNews.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewBag.Title = "Social News";
        }
    }
}
