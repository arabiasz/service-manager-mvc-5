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
    public class TaxOfficesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /TaxOffices/
        public async Task<ActionResult> Index()
        {
            return View(await db.TaxOffices.ToListAsync());
        }

        // GET: /TaxOffices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxOffice taxOffice = await db.TaxOffices.FindAsync(id);
            if (taxOffice == null)
            {
                return HttpNotFound();
            }
            return View(taxOffice);
        }

        // GET: /TaxOffices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /TaxOffices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="TaxOfficeId,Name,Street,HomeNumber,ZipCode,City")] TaxOffice taxOffice)
        {
            if (ModelState.IsValid)
            {
                db.TaxOffices.Add(taxOffice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(taxOffice);
        }

        // GET: /TaxOffices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TaxOffice taxOffice = await db.TaxOffices.FindAsync(id);
            if (taxOffice == null)
            {
                return HttpNotFound();
            }

            return View(taxOffice);
        }

        // POST: /TaxOffices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="TaxOfficeId,Name,Street,HomeNumber,ZipCode,City")] TaxOffice taxOffice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taxOffice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(taxOffice);
        }

        // GET: /TaxOffices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TaxOffice taxOffice = await db.TaxOffices.FindAsync(id);
            if (taxOffice == null)
            {
                return HttpNotFound();
            }

            return PartialView(taxOffice);
        }

        // POST: /TaxOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TaxOffice taxOffice = await db.TaxOffices.FindAsync(id);
            db.TaxOffices.Remove(taxOffice);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                ModelState.AddModelError(String.Empty, "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych");

                return PartialView(taxOffice);
            }

            return Json(new { url = Url.Action("Index", "TaxOffices"), success = true });
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult NameExists(string Name, int taxOfficeId = 0)
        {
            if (db.TaxOffices.Any(x => x.Name.ToLower() == Name.ToLower() && x.TaxOfficeId != taxOfficeId))
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
