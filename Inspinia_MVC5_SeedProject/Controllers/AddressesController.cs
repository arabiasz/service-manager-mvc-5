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
using System.Data.Entity.Infrastructure;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class AddressesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Addresses/
        public async Task<ActionResult> Index(int? id)
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

            ViewBag.CustomerId = id;
            ViewBag.CustomerName = customer.Name;
            var addresses = db.Addresses.Where(a => a.CustomerId == id).OrderBy(a => a.City);

            return View(await addresses.ToListAsync());
        }

        // GET: /Addresses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: /Addresses/Create
        public ActionResult Create(int? customerId)
        {
            if (customerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerName = customer.Name;
            Address address = new Address();
            address.CustomerId = (int)customerId;

            return View(address);
        }

        // POST: /Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="AddressId,Name,Street,HomeNumber,PlaceNumber,ZipCode,PostalBox,Post,City,CustomerId")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = address.CustomerId});
            }

            Customer customer = db.Customers.Find(address.CustomerId);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerName = customer.Name;

            return View(address);
        }

        // GET: /Addresses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }

            Customer customer = db.Customers.Find(address.CustomerId);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerName = customer.Name;

            return View(address);
        }

        // POST: /Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="AddressId,Name,Street,HomeNumber,PlaceNumber,ZipCode,PostalBox,Post,City,CustomerId")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = address.CustomerId });
            }

            Customer customer = db.Customers.Find(address.CustomerId);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerName = customer.Name;

            return View(address);
        }

        // GET: /Addresses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return PartialView(address);
        }

        // POST: /Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            db.Addresses.Remove(address);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                ModelState.AddModelError(String.Empty, "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych");
                return PartialView(address);
            }
            return Json(new { url = Url.Action("Index", "Addresses", new { id = address.CustomerId }), success = true });
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
