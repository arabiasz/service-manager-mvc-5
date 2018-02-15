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
    public class ProducersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Producers/
        public async Task<ActionResult> Index()
        {
            return View(await db.Producers.ToListAsync());
        }

        // GET: /Producers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = await db.Producers.FindAsync(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // GET: /Producers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ProducerId,Name,Street,ZipCode,City,Nip,Regon,Web")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                db.Producers.Add(producer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(producer);
        }

        // GET: /Producers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Producer producer = await db.Producers.FindAsync(id);
            if (producer == null)
            {
                return HttpNotFound();
            }

            return View(producer);
        }

        // POST: /Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ProducerId,Name,Street,ZipCode,City,Nip,Regon,Web")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(producer);
        }

        // GET: /Producers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Producer producer = await db.Producers.FindAsync(id);
            if (producer == null)
            {
                return HttpNotFound();
            }

            return PartialView(producer);
        }

        // POST: /Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Producer producer = await db.Producers.FindAsync(id);
            db.Producers.Remove(producer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                ModelState.AddModelError(String.Empty, "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych");

                return PartialView(producer);
            }

            return Json(new { url = Url.Action("Index", "Producers"), success = true });
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult NameExists(string Name, int ProducerId = 0)
        {
            if (db.Producers.Any(x => x.Name.ToLower() == Name.ToLower() && x.ProducerId != ProducerId))
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
