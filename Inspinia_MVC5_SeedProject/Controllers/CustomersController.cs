using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5_SeedProject.Models;
using Inspinia_MVC5_SeedProject.ViewModels.Customers;
using System.Data.Entity.Infrastructure;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Customers/
        public async Task<ActionResult> Index()
        {
            return View(await db.Customers.ToListAsync());
        }

        // GET: /Customers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: /Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId,TaxOfficeId,Name,Street,HomeNumber,PlaceNumber,ZipCode,PostalBox,Post,City,Nip,Regon,Phone,Web,Email,Country,Province,Community,MainAddressAsInstallation")] CustomerViewModel customerVM)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();
                customer.Name = customerVM.Name;
                customer.Street = customerVM.Street;
                customer.HomeNumber = customerVM.HomeNumber;
                customer.PlaceNumber = customerVM.PlaceNumber;
                customer.PostalBox = customerVM.PostalBox;
                customer.ZipCode = customerVM.ZipCode;
                customer.Post = customerVM.Post;
                customer.City = customerVM.City;

                customer.Nip = customerVM.Nip;
                customer.Regon = customerVM.Regon;
                customer.Phone = customerVM.Phone;
                customer.Web = customerVM.Web;
                customer.Email = customerVM.Email;
                customer.Country = customerVM.Country;
                customer.Province = customerVM.Province;
                customer.Community = customerVM.Community;

                customer.CustomerId = customerVM.CustomerId;

                db.Customers.Add(customer);

                if (customerVM.MainAddressAsInstallation)
                {
                    Address address = new Address();
                    address.Name = customer.Name;
                    address.Street = customer.Street;
                    address.HomeNumber = customer.HomeNumber;
                    address.PlaceNumber = customer.PlaceNumber;
                    address.PostalBox = customer.PostalBox;
                    address.ZipCode = customer.ZipCode;
                    address.Post = customer.Post;
                    address.City = customer.City;
                    address.CustomerId = customer.CustomerId;

                    db.Addresses.Add(address);
                }
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(customerVM);
        }

        // GET: /Customers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.Include(p => p.Addresses).Where(p => p.CustomerId == id).SingleOrDefaultAsync();

            if (customer == null)
            {
                return HttpNotFound();
            }

            CustomerViewModel customerVM = new CustomerViewModel();
            customerVM.Name = customer.Name;
            customerVM.Street = customer.Street;
            customerVM.HomeNumber = customer.HomeNumber;
            customerVM.PlaceNumber = customer.PlaceNumber;
            customerVM.PostalBox = customer.PostalBox;
            customerVM.ZipCode = customer.ZipCode;
            customerVM.Post = customer.Post;
            customerVM.City = customer.City;
            customerVM.Nip = customer.Nip;
            customerVM.Regon = customer.Regon;
            customerVM.Phone = customer.Phone;
            customerVM.Web = customer.Web;
            customerVM.Country = customer.Country;
            customerVM.Province = customer.Province;
            customerVM.Community = customer.Community;
            customerVM.CustomerId = customer.CustomerId;
            customerVM.Email = customer.Email;

            return View(customerVM);
        }

        // POST: /Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CustomerId,TaxOfficeId,Name,Street,HomeNumber,PlaceNumber,ZipCode,PostalBox,Post,City,Nip,Regon,Phone,Web,Email,Country,Province,Community,MainAddressAsInstallation")] CustomerViewModel customerVM)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerVM.CustomerId);
                if (customer == null)
                {
                    return HttpNotFound();
                }

                customer.Name = customerVM.Name;
                customer.Street = customerVM.Street;
                customer.HomeNumber = customerVM.HomeNumber;
                customer.PlaceNumber = customerVM.PlaceNumber;
                customer.PostalBox = customerVM.PostalBox;
                customer.ZipCode = customerVM.ZipCode;
                customer.Post = customerVM.Post;
                customer.City = customerVM.City;
                customer.Nip = customerVM.Nip;
                customer.Regon = customerVM.Regon;
                customer.Phone = customerVM.Phone;
                customer.Web = customerVM.Web;
                customer.Email = customerVM.Email;
                customer.Country = customerVM.Country;
                customer.Province = customerVM.Province;
                customer.Community = customerVM.Community;

                db.Entry(customer).State = EntityState.Modified;

                if (customerVM.MainAddressAsInstallation)
                {
                    Address address = new Address();
                    address.Name = customer.Name;
                    address.Street = customer.Street;
                    address.HomeNumber = customer.HomeNumber;
                    address.PlaceNumber = customer.PlaceNumber;
                    address.PostalBox = customer.PostalBox;
                    address.ZipCode = customer.ZipCode;
                    address.Post = customer.Post;
                    address.City = customer.City;
                    address.CustomerId = customer.CustomerId;

                    db.Addresses.Add(address);
                }

                await db.SaveChangesAsync();

                //info for display alert
                TempData["notice"] = "Changed";
            }
            return View(customerVM);
        }

        // GET: /Customers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return PartialView(customer);
        }

        // POST: /Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException/* ex*/)
            {
                ModelState.AddModelError(string.Empty, "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych");
                return PartialView(customer);
            }
            return Json(new { url = Url.Action("Index", "Customers"), success = true });
        }


        [AllowAnonymous]
        [HttpGet]
        public JsonResult NameExists(string Name, int CustomerId = 0)
        {
            if (db.Customers.Any(x => x.Name.ToLower() == Name.ToLower() && x.CustomerId != CustomerId))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult NipExists(string Nip, int CustomerId = 0)
        {
            if (db.Customers.Any(x => x.Nip == Nip && x.CustomerId != CustomerId))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
