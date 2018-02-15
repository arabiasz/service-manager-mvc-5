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
using Inspinia_MVC5_SeedProject.ViewModels.Fiscalizations;
using System.Globalization;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Data.Entity.Infrastructure;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class FiscalizationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Fiscalizations/
        public async Task<ActionResult> Index()
        {
            var fiscalizations = db.Fiscalizations.Include(f => f.Customer).Include(f => f.TaxOffice).OrderByDescending(d => d.FiscalizationDate);
            return View(await fiscalizations.ToListAsync());
        }

        // GET: /Fiscalizations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Fiscalization fiscalization = await db.Fiscalizations.FindAsync(id);
            if (fiscalization == null)
            {
                return HttpNotFound();
            }

            return View(fiscalization);
        }

        // GET: /Fiscalizations/Create
        public ActionResult Create()
        {
            var fiscalizationViewModel = new FiscalizationViewModel();

            fiscalizationViewModel.FiscalizationDate = DateTime.Now.ToString("yyyy-MM-dd");
            fiscalizationViewModel.UseOfTheDeviceDate = DateTime.Now.ToString("yyyy-MM-dd");
            fiscalizationViewModel.RevievDate = (DateTime.Now).AddYears(2).ToString("yyyy-MM-dd");

            fiscalizationViewModel.Devices = GetDevicesToFiscalization(0, 0);

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name");
            ViewBag.TaxOfficeId = new SelectList(db.TaxOffices, "TaxOfficeId", "Name");

            return View(fiscalizationViewModel);
        }

        // POST: /Fiscalizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FiscalizationId,TaxOfficeId,CustomerId,ModuleId,FiscalizationDate,RevievDate,OrderDate,City,UseOfTheDeviceDate, Devices,Servisman")] FiscalizationViewModel fiscalizationVM)
        {
            //need to be refactor 
            var selectedCount = 0;
            foreach (var item in fiscalizationVM.Devices)
            {
                if (item.Selected)
                    selectedCount++;
            }

            if (selectedCount == 0)
            {
                ModelState.AddModelError("ValidationMessage", "Nie zaznaczono żadnych urządzeń");
                ViewBag.DevicesCountError = "noneSelected";
            }
            else
            {
                for (int i = 0; i < fiscalizationVM.Devices.Count; i++)
                {
                    if (!fiscalizationVM.Devices[i].Selected)
                    {
                        try
                        {
                            ModelState.Remove("Devices[" + i + "].AddressId");
                        }
                        catch (NotSupportedException /*e*/)
                        {

                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                Fiscalization fiscalization = new Fiscalization();

                fiscalization.CustomerId = fiscalizationVM.CustomerId;
                fiscalization.TaxOfficeId = fiscalizationVM.TaxOfficeId;

                fiscalization.FiscalizationDate = DateTime.ParseExact(fiscalizationVM.FiscalizationDate, @"yyyy-MM-dd", CultureInfo.InvariantCulture);
                fiscalization.UseOfTheDeviceDate = DateTime.ParseExact(fiscalizationVM.UseOfTheDeviceDate, @"yyyy-MM-dd", CultureInfo.InvariantCulture);
                fiscalization.RevievDate = DateTime.ParseExact(fiscalizationVM.RevievDate, @"yyyy-MM-dd", CultureInfo.InvariantCulture);

                fiscalization.City = fiscalizationVM.City;
                fiscalization.Servisman = fiscalizationVM.Servisman;

                db.Fiscalizations.Add(fiscalization);

                foreach (var item in fiscalizationVM.Devices)
                {
                    if (item.Selected == true)
                    {
                        var device = db.Devices.Find(item.DeviceId);
                        if (device != null)
                        {
                            device.InterimReviewDate = fiscalization.RevievDate;
                        }

                        var module = db.Modules.Find(item.ModuleId);

                        if (module != null)
                        {
                            module.Status = "FISKALNY";
                            db.FiscalizationsToModules.Add(new FiscalizationToModules() { FiscalizationId = fiscalization.FiscalizationId, ModuleId = module.ModuleId, AddressId = item.AddressId });
                            db.Entry(module).State = EntityState.Modified;
                        }
                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", fiscalizationVM.CustomerId);
            ViewBag.TaxOfficeId = new SelectList(db.TaxOffices, "TaxOfficeId", "Name", fiscalizationVM.TaxOfficeId);

            var adress = db.Addresses.Where(i => i.CustomerId == fiscalizationVM.CustomerId).ToList();
            foreach (var item in fiscalizationVM.Devices)
            {
                item.Addresses = adress;
            }

            return View(fiscalizationVM);
        }

        // GET: /Fiscalizations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fiscalization fiscalization = await db.Fiscalizations.FindAsync(id);
            if (fiscalization == null)
            {
                return HttpNotFound();
            }

            FiscalizationViewModel model = new FiscalizationViewModel();
            model.FiscalizationDate = fiscalization.FiscalizationDate.ToString("yyyy-MM-dd");
            model.RevievDate = fiscalization.RevievDate.ToString("yyyy-MM-dd");
            model.UseOfTheDeviceDate = fiscalization.UseOfTheDeviceDate.ToString("yyyy-MM-dd");
            model.City = fiscalization.City;
            model.CustomerId = fiscalization.CustomerId;
            model.FiscalizationId = fiscalization.FiscalizationId;
            model.Servisman = fiscalization.Servisman;

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", fiscalization.CustomerId);
            ViewBag.TaxOfficeId = new SelectList(db.TaxOffices, "TaxOfficeId", "Name", fiscalization.TaxOfficeId);

            //select fiscalized (current order) devices and all with status 'NIEFISKALNY'
            var devices = (from d in db.Devices
                           join m in db.Modules on d.DeviceId equals m.DeviceId
                           join f in db.DevicesFolders on d.DevicesFolderId equals f.DevicesFolderId
                           join fm in db.FiscalizationsToModules.DefaultIfEmpty() on m.ModuleId equals fm.ModuleId into lrs
                           from lr in lrs.DefaultIfEmpty()
                           where ((m.Status == "NIEFISKALNY") || (m.Status == "FISKALNY") & (lr.FiscalizationId == fiscalization.FiscalizationId))
                           select new SelectDeviceToFiscalizationViewModel()
                           {
                               DeviceId = d.DeviceId,
                               ModuleId = m.ModuleId,
                               AddressId = (from fm in db.FiscalizationsToModules
                                            where (fm.FiscalizationId == fiscalization.FiscalizationId) && (m.ModuleId == fm.ModuleId)
                                            select fm.AddressId).FirstOrDefault(),
                               DeviceName = f.Name,
                               SerialNumber = d.SerialNumber,
                               UniqueNumber = m.UniqueNumber,
                               Status = m.Status,
                               Selected = ((from fm in db.FiscalizationsToModules
                                            where (fm.FiscalizationId == fiscalization.FiscalizationId) && (m.ModuleId == fm.ModuleId)
                                            select fm).Count() > 0),
                               Addresses = db.Addresses.Where(i => i.CustomerId == model.CustomerId).ToList()
                           }).ToList();

            model.Devices = devices;

            return View(model);
        }

        // POST: /Fiscalizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FiscalizationId,TaxOfficeId,CustomerId,ModuleId,FiscalizationDate,RevievDate,OrderDate,City,UseOfTheDeviceDate, Devices,Servisman")] FiscalizationViewModel model)
        {
            var selectedCount = 0;
            foreach (var item in model.Devices)
            {
                if (item.Selected)
                    selectedCount++;
            }

            if (selectedCount == 0)
            {
                ModelState.AddModelError("ValidationMessage", "Nie zaznaczono żadnych urządzeń");
                ViewBag.DevicesCountError = "Nie zaznaczono żadnych urządzeń";
            }
            else
            {
                for (int i = 0; i < model.Devices.Count; i++)
                {
                    if (model.Devices[i].Selected && (model.Devices[i].AddressId == 0))
                        ModelState.AddModelError("Devices[" + i + "].AddressId", "Wybierz miejsce instalacji");
                    else
                    {
                        try
                        {
                            ModelState.Remove("Devices[" + i + "].AddressId");
                        }
                        catch (NotSupportedException /*e*/)
                        {

                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                Fiscalization fiscalization = db.Fiscalizations.FirstOrDefault(f => f.FiscalizationId == model.FiscalizationId);
                if (fiscalization == null)
                {
                    return HttpNotFound();
                }

                fiscalization.CustomerId = model.CustomerId;
                fiscalization.TaxOfficeId = model.TaxOfficeId;

                fiscalization.FiscalizationDate = DateTime.ParseExact(model.FiscalizationDate, @"yyyy-MM-dd", CultureInfo.InvariantCulture);
                fiscalization.UseOfTheDeviceDate = DateTime.ParseExact(model.UseOfTheDeviceDate, @"yyyy-MM-dd", CultureInfo.InvariantCulture);
                fiscalization.RevievDate = DateTime.ParseExact(model.RevievDate, @"yyyy-MM-dd", CultureInfo.InvariantCulture);

                fiscalization.City = model.City;
                fiscalization.Servisman = model.Servisman;

                foreach (var item in db.FiscalizationsToModules)
                {
                    if (item.FiscalizationId == fiscalization.FiscalizationId)
                    {
                        Module module = db.Modules.Find(item.ModuleId);
                        module.Status = "NIEFISKALNY";
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                }

                var selectedIds = model.GetSelectedIds();

                foreach (var item in selectedIds)
                {
                    var device = db.Devices.Find(item.DeviceId);
                    if (device != null)
                    {
                        device.InterimReviewDate = fiscalization.RevievDate;
                    }

                    var module = db.Modules.Find(item.ModuleId);
                    if (module != null)
                    {
                        module.Status = "FISKALNY";
                        db.FiscalizationsToModules.Add(new FiscalizationToModules() { FiscalizationId = fiscalization.FiscalizationId, ModuleId = module.ModuleId, AddressId = item.AddressId });
                        db.Entry(module).State = EntityState.Modified;
                    }
                }

                db.Entry(fiscalization).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", model.CustomerId);
            ViewBag.TaxOfficeId = new SelectList(db.TaxOffices, "TaxOfficeId", "Name", model.TaxOfficeId);

            var adress = db.Addresses.Where(i => i.CustomerId == model.CustomerId).ToList();
            foreach (var item in model.Devices)
            {
                item.Addresses = adress;
            }
            return View(model);
        }

        // GET: /Fiscalizations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Fiscalization fiscalization = await db.Fiscalizations.FindAsync(id);
            if (fiscalization == null)
            {
                return HttpNotFound();
            }

            return View(fiscalization);
        }

        // POST: /Fiscalizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Fiscalization fiscalization = await db.Fiscalizations.FindAsync(id);

            var fiscalizationToModules = await db.FiscalizationsToModules.Where(i => i.FiscalizationId == id).ToListAsync();
            foreach(var item in fiscalizationToModules)
            {
                Module m = await db.Modules.FindAsync(item.ModuleId);
                if(!CanBeDeleted(m.ModuleId))
                {
                    ModelState.AddModelError(String.Empty, "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych");
                    return PartialView(fiscalization);
                }
                m.Status = "NIEFISKALNY";
                db.Entry(m).State = EntityState.Modified;
            }

            db.Fiscalizations.Remove(fiscalization);
            
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                string errorMessage = "";
                errorMessage = "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych";

                ModelState.AddModelError(String.Empty, errorMessage);
                return PartialView(fiscalization);
            }

            return Json(new { url = Url.Action("Index", "Fiscalizations"), success = true });
        }

        bool CanBeDeleted(int id)
        {
            int count = db.ReadingOfFiscalMemories.Where(m => m.ModuleId == id).Count() + db.InterimReviews.Where(m => m.ModuleId == id).Count() 
                        + db.ServiceInterventions.Where(m => m.ModuleId == id).Count();
            if (count > 0)
            {
                return false;
            }
            return true;
        }

        List<SelectDeviceToFiscalizationViewModel> GetDevicesToFiscalization(int customerId, int fiscalizationId)
        {
            Fiscalization fiscalization = db.Fiscalizations.Find(fiscalizationId);
            List<SelectDeviceToFiscalizationViewModel> devices;

            if (fiscalization != null)
            {
                devices = (from d in db.Devices
                           join m in db.Modules on d.DeviceId equals m.DeviceId
                           join f in db.DevicesFolders on d.DevicesFolderId equals f.DevicesFolderId
                           join fm in db.FiscalizationsToModules.DefaultIfEmpty() on m.ModuleId equals fm.ModuleId into lrs
                           from lr in lrs.DefaultIfEmpty()
                           where ((m.Status == "NIEFISKALNY") || (m.Status == "FISKALNY") & (lr.FiscalizationId == fiscalization.FiscalizationId))
                           select new SelectDeviceToFiscalizationViewModel()
                           {
                               DeviceId = d.DeviceId,
                               ModuleId = m.ModuleId,
                               AddressId = (from fm in db.FiscalizationsToModules
                                            where (fm.FiscalizationId == fiscalization.FiscalizationId) && (m.ModuleId == fm.ModuleId)
                                            select fm.AddressId).FirstOrDefault(),
                               DeviceName = f.Name,
                               SerialNumber = d.SerialNumber,
                               UniqueNumber = m.UniqueNumber,
                               Status = m.Status,
                               Selected = ((from fm in db.FiscalizationsToModules
                                            where (fm.FiscalizationId == fiscalization.FiscalizationId) && (m.ModuleId == fm.ModuleId)
                                            select fm).Count() > 0),
                               Addresses = db.Addresses.Where(i => i.CustomerId == customerId).ToList()
                           }).ToList();
            }
            else
            {
                devices = (from d in db.Devices
                           join m in db.Modules on d.DeviceId equals m.DeviceId
                           join f in db.DevicesFolders on d.DevicesFolderId equals f.DevicesFolderId
                           where m.Status == "NIEFISKALNY"
                           select new SelectDeviceToFiscalizationViewModel()
                           {
                               DeviceId = d.DeviceId,
                               ModuleId = m.ModuleId,
                               DeviceName = f.Name,
                               SerialNumber = d.SerialNumber,
                               UniqueNumber = m.UniqueNumber,
                               Status = m.Status,
                           }).ToList();
            }

            return devices;
        }

        public PartialViewResult SetDevicesAddressesByCustomer(int customerId, int fiscalizationId)
        {
            var modelView = new FiscalizationViewModel();

            var adress = db.Addresses.Where(i => i.CustomerId == customerId).ToList();

            modelView.Devices = GetDevicesToFiscalization(customerId, fiscalizationId);

            foreach (var item in modelView.Devices)
            {
                item.Addresses = adress;
            }

            return PartialView("~/Views/Fiscalizations/EditorTemplates/SelectDeviceToFiscalizationViewModel.cshtml", modelView);
        }

        public async Task<ActionResult> GetReportPdf(int id)
        {
            Fiscalization fisc = await db.Fiscalizations.FindAsync(id);
            if (fisc == null)
            {
                return HttpNotFound();
            }

            XtraReport reportService = new XtraReport();
            string path = Server.MapPath("~/Reports/zaw_us_serwis.repx");
            reportService.LoadLayout(path);
            reportService.Parameters["FISCALIZATIONID"].Value = id;
            reportService.CreateDocument();

            XtraReport reportCustomer = new XtraReport();
            string path1 = Server.MapPath("~/Reports/zaw_us_podatnik.repx");
            reportCustomer.LoadLayout(path1);
            reportCustomer.Parameters["FISCALIZATIONID"].Value = id;
            reportCustomer.CreateDocument();

            reportService.Pages.AddRange(reportCustomer.Pages);

            reportService.PrintingSystem.ContinuousPageNumbering = false;

            var stream = new MemoryStream();
            reportService.ExportToPdf(stream);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "zlecenie_us.pdf",
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(stream.GetBuffer(), "application/pdf");
        }

        //bool CanBeDeleted(List<int> ids)
        //{
        //    //TODO
        //    foreach(var item in ids)
        //    {
                
        //    }

        //    return true;
        //}

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
