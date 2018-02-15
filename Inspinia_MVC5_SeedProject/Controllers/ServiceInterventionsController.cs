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
using Inspinia_MVC5_SeedProject.ViewModels.ServiceInterventions;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class ServiceInterventionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ServiceInterventions/
        public async Task<ActionResult> Index()
        {
            var serviceinterventions = (from s in db.ServiceInterventions
                                        join m in db.Modules on s.ModuleId equals m.ModuleId
                                        join d in db.Devices on m.DeviceId equals d.DeviceId
                                        join f in db.DevicesFolders on d.DevicesFolderId equals f.DevicesFolderId
                                        join fm in db.FiscalizationsToModules on m.ModuleId equals fm.ModuleId
                                        join fs in db.Fiscalizations on fm.FiscalizationId equals fs.FiscalizationId
                                        join c in db.Customers on fs.CustomerId equals c.CustomerId
                                        //where (m.Active == true)
                                        orderby s.InterventionStart
                                        select new ServiceInterventionsViewModel()
                                        {
                                            ModelName = f.Name,
                                            SerialNumber = d.SerialNumber,
                                            UniqueNumber = m.UniqueNumber,
                                            CustomerName = c.Name,
                                            CustomerNip = c.Nip,
                                            InterventionStart = s.InterventionStart,
                                            InterventionEnd = s.InterventionEnd,
                                            ServiceInterventionId = s.ServiceInterventionId,
                                            Status = s.InterventionEnd == null ? "OTWARTE" : "ZAMKNIETE"
                                        }).ToListAsync();

            return View(await serviceinterventions);
        }

        // GET: /ServiceInterventions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceIntervention serviceIntervention = await db.ServiceInterventions.FindAsync(id);
            if (serviceIntervention == null)
            {
                return HttpNotFound();
            }

            return View(serviceIntervention);
        }

        // GET: /ServiceInterventions/Create
        public ActionResult Create(int? deviceId)
        {
            var module = db.Modules.Include(d => d.Device.DevicesFolder).Where(d => d.DeviceId == deviceId).Where(m => m.Active == true).FirstOrDefault();

            if (module == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceIntervention newService = new ServiceIntervention();
            newService.ModuleId = module.ModuleId;

            return View(newService);
        }

        // POST: /ServiceInterventions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ServiceInterventionId,ModuleId,InterventionStart,InterventionEnd,ReceiptsFiscalCountStart,ReceiptsFiscalCountEnd,FiscalDailyReportStart,FiscalDailyReportEnd,ResettingRamCountStart,ResettingRamCountEnd,ReceiptsCountAllStart,ReceiptsCountAllEnd,ProblemsDescrtiption,SealCount,SealCondition,RepairedComponents,FiscalDocPrinted,WhyCantRepairAtCustomer,PlaceOfRepair,ConfirmationOfReceipt,ServiceBookPageNumber")] ServiceIntervention serviceIntervention)
        {
            if (ModelState.IsValid)
            {
                db.ServiceInterventions.Add(serviceIntervention);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "UniqueNumber", serviceIntervention.ModuleId);
            return View(serviceIntervention);
        }

        // GET: /ServiceInterventions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceIntervention serviceIntervention = await db.ServiceInterventions.FindAsync(id);
            if (serviceIntervention == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "UniqueNumber", serviceIntervention.ModuleId);
            return View(serviceIntervention);
        }

        // POST: /ServiceInterventions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ServiceInterventionId,ModuleId,InterventionStart,InterventionEnd,ReceiptsFiscalCountStart,ReceiptsFiscalCountEnd,FiscalDailyReportStart,FiscalDailyReportEnd,ResettingRamCountStart,ResettingRamCountEnd,ReceiptsCountAllStart,ReceiptsCountAllEnd,ProblemsDescrtiption,SealCount,SealCondition,RepairedComponents,FiscalDocPrinted,WhyCantRepairAtCustomer,PlaceOfRepair,ConfirmationOfReceipt,ServiceBookPageNumber")] ServiceIntervention serviceIntervention)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceIntervention).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "UniqueNumber", serviceIntervention.ModuleId);

            return View(serviceIntervention);
        }

        // GET: /ServiceInterventions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceIntervention serviceIntervention = await db.ServiceInterventions.FindAsync(id);
            if (serviceIntervention == null)
            {
                return HttpNotFound();
            }

            return PartialView(serviceIntervention);
        }

        // POST: /ServiceInterventions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceIntervention serviceIntervention = await db.ServiceInterventions.FindAsync(id);
            db.ServiceInterventions.Remove(serviceIntervention);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                string errorMessage = "";
                errorMessage = "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych";

                ModelState.AddModelError(String.Empty, errorMessage);

                return PartialView(serviceIntervention);
            }
            return Json(new { url = Url.Action("Index", "ServiceInterventions"), success = true });
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
