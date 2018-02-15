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
    public class DevicesFoldersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /DevicesFolders/
        public async Task<ActionResult> Index()
        {
            var devicesfolders = db.DevicesFolders.Include(d => d.Group).Include(d => d.Producer);
            return View(await devicesfolders.ToListAsync());
        }

        // GET: /DevicesFolders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevicesFolder devicesFolder = await db.DevicesFolders.FindAsync(id);
            if (devicesFolder == null)
            {
                return HttpNotFound();
            }
            return View(devicesFolder);
        }

        // GET: /DevicesFolders/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name");
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name");
            return View();
        }

        // POST: /DevicesFolders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="DevicesFolderId,Name,ProducerId,GroupId")] DevicesFolder devicesFolder)
        {
            if (ModelState.IsValid)
            {
                db.DevicesFolders.Add(devicesFolder);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name", devicesFolder.GroupId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", devicesFolder.ProducerId);
            return View(devicesFolder);
        }

        // GET: /DevicesFolders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DevicesFolder devicesFolder = await db.DevicesFolders.FindAsync(id);
            if (devicesFolder == null)
            {
                return HttpNotFound();
            }

            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name", devicesFolder.GroupId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", devicesFolder.ProducerId);

            return View(devicesFolder);
        }

        // POST: /DevicesFolders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="DevicesFolderId,Name,ProducerId,GroupId")] DevicesFolder devicesFolder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(devicesFolder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "Name", devicesFolder.GroupId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", devicesFolder.ProducerId);

            return View(devicesFolder);
        }

        // GET: /DevicesFolders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DevicesFolder devicesFolder = await db.DevicesFolders.FindAsync(id);
            if (devicesFolder == null)
            {
                return HttpNotFound();
            }

            return PartialView(devicesFolder);
        }

        // POST: /DevicesFolders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DevicesFolder devicesFolder = await db.DevicesFolders.FindAsync(id);
            db.DevicesFolders.Remove(devicesFolder);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                ModelState.AddModelError(String.Empty, "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych");

                return PartialView(devicesFolder);
            }

            return Json(new { url = Url.Action("Index", "DevicesFolders"), success = true });
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult NameExists(string Name, int DevicesFolderId = 0)
        {
            if (db.DevicesFolders.Any(x => x.Name.ToLower() == Name.ToLower() && x.DevicesFolderId != DevicesFolderId))
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
