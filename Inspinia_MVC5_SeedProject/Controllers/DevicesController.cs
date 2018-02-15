using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using Inspinia_MVC5_SeedProject.Models;
using Inspinia_MVC5_SeedProject.ViewModels.Devices;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class DevicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Devices/
        public async Task<ActionResult> Index()
        {
            var devices = await GetAllDevices();
            return View(devices);
        }

        // GET: /Devices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = await db.Devices.FindAsync(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // GET: /Devices/Create
        public ActionResult Create()
        {
            var df = db.DevicesFolders.Include(c => c.Producer).ToList();
            ViewBag.DevicesList = new SelectList(df, "DevicesFolderId", "Name", "Producer.Name", -1);

            return View();
        }

        // POST: /Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Module module)
        {
            if (ModelState.IsValid)
            {
                module.Active = true;
                module.Status = "NIEFISKALNY";
                module.Device.SerialNumber = module.Device.SerialNumber.ToUpper();
                module.UniqueNumber = module.UniqueNumber.ToUpper();

                db.Devices.Add(module.Device);
                db.Modules.Add(module);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Devices");
            }

            var df = db.DevicesFolders.Include(c => c.Producer).ToList();
            ViewBag.DevicesList = new SelectList(df, "DevicesFolderId", "Name", "Producer.Name");

            return View(module);
        }

        // GET: /Devices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Device device = await db.Devices.FindAsync(id);
            if (device == null)
            {
                return HttpNotFound();
            }

            var deviceToEdit = await (from d in db.Devices
                       join m in db.Modules on d.DeviceId equals m.DeviceId
                       join df in db.DevicesFolders on d.DevicesFolderId equals df.DevicesFolderId
                       where ((d.DeviceId == device.DeviceId) && (m.Active == true))
                       select new DevicesViewModel()
                       {
                           DeviceId = d.DeviceId,
                           DevicesFolderId = df.DevicesFolderId,
                           DeviceName = df.Name,
                           SerialNumber = d.SerialNumber,
                           UniqueNumber = m.UniqueNumber,
                           RegistrationNumber = d.RegistrationNumber,
                           ReviewInterval = d.ReviewInterval,
                           WarrantyInterval = d.WarrantyInterval
                       }).FirstOrDefaultAsync();

            var devices = db.DevicesFolders.Include(c => c.Producer).ToList();
            ViewBag.DevicesList = new SelectList(devices, "DevicesFolderId", "Name", "Producer.Name");

            return View(deviceToEdit);
        }

        // POST: /Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(/*[Bind(Include="DeviceId,DevicesFolderId,SerialNumber,RegistrationNumber,ReviewInterval,WarrantyInterval,InterimReviewDate")]*/ DevicesViewModel device)
        {
            if (ModelState.IsValid)
            {
                Device dev = db.Devices.FirstOrDefault(d => d.DeviceId == device.DeviceId);
                Module module = await db.Modules.Where(m => m.DeviceId == device.DeviceId && m.Active == true).FirstOrDefaultAsync();
                if (dev == null || module == null)
                {
                    return HttpNotFound();
                }

                dev.DevicesFolderId = device.DevicesFolderId;
                dev.SerialNumber = device.SerialNumber;
                dev.RegistrationNumber = device.RegistrationNumber;
                dev.ReviewInterval = device.ReviewInterval;
                dev.WarrantyInterval = device.WarrantyInterval;

                module.UniqueNumber = device.UniqueNumber;

                db.Entry(dev).State = EntityState.Modified;
                db.Entry(module).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            var dfList = await db.DevicesFolders.Include(c => c.Producer).ToListAsync();
            ViewBag.DevicesList = new SelectList(dfList, "DevicesFolderId", "Name", "Producer.Name");

            return View(device);
        }
    

        // GET: /Devices/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return PartialView(device);
        }

        // POST: /Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Device device = await db.Devices.Include(df => df.DevicesFolder).SingleAsync(i => i.DeviceId == id);
            db.Devices.Remove(device);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                string errorMessage = "";
                //if (ex.InnerException.Number == 547)
                    errorMessage = "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych";
                //else
                //    errorMessage = "Błąd bazy danych";

                ModelState.AddModelError(String.Empty, errorMessage);
                return PartialView(device);
            }

            return Json(new { url = Url.Action("Index", "Devices"), success = true });
        }

        private async Task<List<DevicesViewModel>> GetAllDevices()
        {
            return await Task.Run(() => (from d in db.Devices
                                   join m in db.Modules on d.DeviceId equals m.DeviceId
                                   join f in db.DevicesFolders on d.DevicesFolderId equals f.DevicesFolderId
                                   where (m.Active == true)
                                   orderby d.InterimReviewDate
                                   select new DevicesViewModel()
                                   {
                                       DeviceId = d.DeviceId,
                                       DeviceName = f.Name,
                                       SerialNumber = d.SerialNumber,
                                       UniqueNumber = m.UniqueNumber,
                                       RegistrationNumber = d.RegistrationNumber,
                                       Status = m.Status,
                                       ReviewInterval = d.ReviewInterval,
                                       WarrantyInterval = d.WarrantyInterval,
                                       InterimReviewDate = d.InterimReviewDate
                                   }).ToListAsync());
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
