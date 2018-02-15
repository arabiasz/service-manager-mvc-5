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

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class DealersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Dealers/
        public async Task<ActionResult> Index()
        {
            Dealer d = await db.Dealers.SingleOrDefaultAsync();
            if (d == null)
            {
                return RedirectToAction("Create");
            }
            else
                return RedirectToAction("Edit", new { id = d.DealerId });
        }

        // GET: /Dealers/Create
        public ActionResult Create()
        {
            Dealer d = db.Dealers.SingleOrDefault();
            if (d == null)
                return View();
            else
                return RedirectToAction("Edit", new { id = d.DealerId });
        }

        // POST: /Dealers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="DealerId,Name,Street,HomeNumber,PlaceNumber,ZipCode,PostalBox,Post,City,Nip,Regon,Phone,Email,Country,Province,Community")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Dealers.Add(dealer);
                await db.SaveChangesAsync();

                //info for display alert
                TempData["notice"] = "Changed";
                return RedirectToAction("Index");
            }

            return View(dealer);
        }

        // GET: /Dealers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Dealer dealer = await db.Dealers.FindAsync(id);

            if (dealer == null)
            {
                return HttpNotFound();
            }

            return View(dealer);
        }

        // POST: /Dealers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="DealerId,Name,Street,HomeNumber,PlaceNumber,ZipCode,PostalBox,Post,City,Nip,Regon,Phone,Email,Country,Province,Community")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                await db.SaveChangesAsync();

                //info for display alert
                TempData["notice"] = "Changed";
                return RedirectToAction("Index");
            }

            return View(dealer);
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
