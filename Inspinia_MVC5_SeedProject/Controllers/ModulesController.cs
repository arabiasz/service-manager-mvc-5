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
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Modules/
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = await db.Devices.Include(df => df.DevicesFolder).SingleAsync(i => i.DeviceId == id);
            if (device == null)
            {
                return HttpNotFound();
            }

            ViewBag.DeviceId = id;
            ViewBag.DeviceName = device.DevicesFolder.Name;
            ViewBag.DeviceSerialNumber = device.SerialNumber;

            var modules = db.Modules.Where(a => a.DeviceId == id).OrderBy(a => a.Status);
            foreach(var item in modules)
            {
                if (item.Active && item.Status == "ODCZYT")
                {
                    ViewBag.Dodaj = "TAK";
                }
                else
                    ViewBag.Dodaj = "NIE";
            }


            return View(await modules.ToListAsync());
        }

        // GET: /Modules/Create
        public ActionResult Create(int? deviceId)
        {
            if (deviceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(deviceId);
            if (device == null)
            {
                return HttpNotFound();
            }

            Module module = new Module();
            module.DeviceId = (int)deviceId;

            return View(module);
        }

        // POST: /Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ModuleId,DeviceId,UniqueNumber,Active,Status")] Module module)
        {
            if (ModelState.IsValid)
            {
                Module currentActive = db.Modules.Where(m => m.Active == true).SingleOrDefault(d => d.DeviceId == module.DeviceId);
                if(currentActive == null)
                { 
                    return HttpNotFound();
                }
                currentActive.Active = false;
                module.Active = true;
                module.Status = "NIEFISKALNY";

                db.Entry(currentActive).State = EntityState.Modified;
                db.Modules.Add(module);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = module.DeviceId });
            }

            return View(module);
        }

        // GET: /Modules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: /Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ModuleId,DeviceId,UniqueNumber,Active,Status")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = module.DeviceId });
            }
            return View(module);
        }

        // GET: /Modules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return PartialView(module);
        }

        // POST: /Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Module module = await db.Modules.FindAsync(id);
            int count = db.Modules.Where(d => d.DeviceId == module.DeviceId).Count();
            if ((!module.Active) && (module.Status != "NIEFISKALNY") || (count == 1) && (module.Status == "NIEFISKALNY"))
            {
                ModelState.AddModelError(String.Empty, "Nie można usunąć modułu");
                return PartialView(module);
            }

            db.Modules.Remove(module);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                ModelState.AddModelError(String.Empty, "Nie można usunąć modułu");
                return PartialView(module);
            }

            if (count > 1)
            {
                Module changeActiveModule = await db.Modules.Where(m => m.Active == false).OrderByDescending(o => o.ModuleId).FirstAsync(d => d.DeviceId == module.DeviceId);
                if(changeActiveModule == null)
                {
                    return HttpNotFound();
                }
                changeActiveModule.Active = true;
                db.Entry(changeActiveModule).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "error???");
                    return PartialView(module);
                }
            }

            return Json(new { url = Url.Action("Index", "Modules", new { id = module.DeviceId }), success = true });
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
