using Inspinia_MVC5_SeedProject.Models;
using Inspinia_MVC5_SeedProject.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var home = new HomeViewModel();

            home.interimReviews = db.InterimReviews.ToList();
            home.serviesInterventions = db.ServiceInterventions.ToList();

            return View(home);
        }
    }
}