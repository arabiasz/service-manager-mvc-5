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
using DevExpress.XtraReports.UI;
using System.IO;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class ReadingOfFiscalMemoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ReadingOfFiscalMemories/
        public async Task<ActionResult> Index()
        {
            return View(await db.ReadingOfFiscalMemories.ToListAsync());
        }

        // GET: /ReadingOfFiscalMemories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReadingOfFiscalMemory readingOfFiscalMemory = await db.ReadingOfFiscalMemories.FindAsync(id);
            if (readingOfFiscalMemory == null)
            {
                return HttpNotFound();
            }
            return View(readingOfFiscalMemory);
        }

        // GET: /ReadingOfFiscalMemories/Create
        public ActionResult Create(int? deviceId)
        {
            if (deviceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var deviceReadMemoryDetails = (from d in db.Devices
                                           join m in db.Modules on d.DeviceId equals m.DeviceId
                                           join df in db.DevicesFolders on d.DevicesFolderId equals df.DevicesFolderId
                                           join fm in db.FiscalizationsToModules on m.ModuleId equals fm.ModuleId
                                           join a in db.Addresses on fm.AddressId equals a.AddressId
                                           join f in db.Fiscalizations on fm.FiscalizationId equals f.FiscalizationId
                                           join c in db.Customers on f.CustomerId equals c.CustomerId
                                           where ((m.Status == "FISKALNY") & (m.Active == true))
                                           select new
                                           {
                                               ModuleId = m.ModuleId,
                                               DeviceName = df.Name,
                                               SerialNumber = d.SerialNumber,
                                               UniqueNumber = m.UniqueNumber,
                                               RegistrationNumber = d.RegistrationNumber,
                                               DateOdFiscalization = f.FiscalizationDate,
                                               //nie uwzglednia numeru lokalu
                                               InstallationAddress = a.City == a.Post ? (a.Street == null ? a.City + " " + a.HomeNumber + ", " + a.ZipCode + " " + a.Post : "ul. " + a.Street + " " + a.HomeNumber + ", " + a.ZipCode + " " + a.Post):
                                                                                        (a.Street == null ? a.City + " " + a.HomeNumber + ", " + a.ZipCode + " " + a.Post : a.City + ", ul. " + a.Street + " " + a.HomeNumber + ", " + a.ZipCode + " " + a.Post),
                                               CustomerName = c.Name,
                                               CustomerNIP = c.Nip,
                                               CustomerAddress = c.City == c.Post ? (c.Street == null ? c.City + " " + c.HomeNumber + ", " + c.ZipCode + " " + c.Post : "ul. " + c.Street + " " + c.HomeNumber + ", " + c.ZipCode + " " + c.Post) :
                                                                                        (c.Street == null ? c.City + " " + c.HomeNumber + ", " + c.ZipCode + " " + c.Post : c.City + ", ul. " + c.Street + " " + c.HomeNumber + ", " + c.ZipCode + " " + c.Post),
                                           }).SingleOrDefault();

            if (deviceReadMemoryDetails == null)
            {
                return HttpNotFound();
            }

            ReadingOfFiscalMemory readMemory = new ReadingOfFiscalMemory();
            readMemory.ModuleId = deviceReadMemoryDetails.ModuleId;
            readMemory.SerialNumber = deviceReadMemoryDetails.SerialNumber;
            readMemory.UniqueNumber = deviceReadMemoryDetails.UniqueNumber;
            readMemory.RegistrationNumber = deviceReadMemoryDetails.RegistrationNumber;
            readMemory.FromReportDate = readMemory.DateOfFiscalization = deviceReadMemoryDetails.DateOdFiscalization.Date;

            readMemory.ToReportDate = null;//DateTime.Now.Date;
            readMemory.DateOfCompletion = DateTime.Now.Date;

            readMemory.InstallationAddress = deviceReadMemoryDetails.InstallationAddress;
            readMemory.CustomerName = deviceReadMemoryDetails.CustomerName;
            readMemory.CustomerNIP = deviceReadMemoryDetails.CustomerNIP;
            readMemory.CustomerAddress = deviceReadMemoryDetails.CustomerAddress;

            ViewBag.VatOld = GetVatOldList();
            ViewBag.VatNew = GetVatNewList();
            ViewBag.VatZW = GetVatZWList();

            return View(readMemory);
        }

        // POST: /ReadingOfFiscalMemories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include= "ReadingOfFiscalMemoryId,ModuleId,CustomerName,CustomerNIP,CustomerAddress,InstallationAddress,UniqueNumber,SerialNumber," +
            "RegistrationNumber,DateOfFiscalization,ReadingReason,ReadingType,FromReport,ToReport,FromReportDate,ToReportDate," +
            "S_PTU_A_old,S_PTU_B_old,S_PTU_C_old,S_PTU_D_old,S_PTU_E_old,S_PTU_F_old,S_PTU_G_old," +
            "VAT_A_old,VAT_B_old,VAT_C_old,VAT_D_old,VAT_E_old,VAT_F_old,VAT_G_old," +
            "PTU_A_old,PTU_B_old,PTU_C_old,PTU_D_old,PTU_E_old,PTU_F_old," +
            "S_PTU_A_new,S_PTU_B_new,S_PTU_C_new,S_PTU_D_new,S_PTU_E_new,S_PTU_F_new,S_PTU_G_new," +
            "VAT_A_new,VAT_B_new,VAT_C_new,VAT_D_new,VAT_E_new,VAT_F_new,VAT_G_new," +
            "PTU_A_new,PTU_B_new,PTU_C_new,PTU_D_new,PTU_E_new,PTU_F_new," +
            "TotalAmount,TotalAmountPTU,MemoryResetCount,NumberOfFiscalRecipt,NumberOfReciptCanceled,ValueOfCanceledRecipt,Addnotations,DatesOfInterimReviews,City,DateOfCompletion")] ReadingOfFiscalMemory readingOfFiscalMemory)
        {
            if (ModelState.IsValid)
            {
                Module module = await db.Modules.FindAsync(readingOfFiscalMemory.ModuleId);
                if (module == null)
                {
                    return HttpNotFound();
                }
                //module.Active = false;
                module.Status = "ODCZYT";
                db.Entry(module).State = EntityState.Modified;

                db.ReadingOfFiscalMemories.Add(readingOfFiscalMemory);
   
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.VatOld = GetVatOldList();
            ViewBag.VatNew = GetVatNewList();
            ViewBag.VatZW = GetVatZWList();

            return View(readingOfFiscalMemory);
        }

        // GET: /ReadingOfFiscalMemories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReadingOfFiscalMemory readingOfFiscalMemory = await db.ReadingOfFiscalMemories.FindAsync(id);
            if (readingOfFiscalMemory == null)
            {
                return HttpNotFound();
            }

            ViewBag.VatOld = GetVatOldList();
            ViewBag.VatNew = GetVatNewList();
            ViewBag.VatZW = GetVatZWList();

            return View(readingOfFiscalMemory);
        }

        // POST: /ReadingOfFiscalMemories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ReadingOfFiscalMemoryId,ModuleId,CustomerName,CustomerNIP,CustomerAddress,InstallationAddress,UniqueNumber,SerialNumber,RegistrationNumber,DateOfFiscalization,ReadingReason,ReadingType,FromReport,ToReport,FromReportDate,ToReportDate,S_PTU_A_old,S_PTU_B_old,S_PTU_C_old,S_PTU_D_old,S_PTU_E_old,S_PTU_F_old,S_PTU_G_old,VAT_A_old,VAT_B_old,VAT_C_old,VAT_D_old,VAT_E_old,VAT_F_old,VAT_G_old,PTU_A_old,PTU_B_old,PTU_C_old,PTU_D_old,PTU_E_old,PTU_F_old,S_PTU_A_new,S_PTU_B_new,S_PTU_C_new,S_PTU_D_new,S_PTU_E_new,S_PTU_F_new,S_PTU_G_new,VAT_A_new,VAT_B_new,VAT_C_new,VAT_D_new,VAT_E_new,VAT_F_new,VAT_G_new,PTU_A_new,PTU_B_new,PTU_C_new,PTU_D_new,PTU_E_new,PTU_F_new,TotalAmount,TotalAmountPTU,MemoryResetCount,NumberOfFiscalRecipt,NumberOfReciptCanceled,ValueOfCanceledRecipt,Addnotations,DatesOfInterimReviews,City,DateOfCompletion")] ReadingOfFiscalMemory readingOfFiscalMemory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(readingOfFiscalMemory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.VatOld = GetVatOldList();
            ViewBag.VatNew = GetVatNewList();
            ViewBag.VatZW = GetVatZWList();

            return View(readingOfFiscalMemory);
        }

        // GET: /ReadingOfFiscalMemories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReadingOfFiscalMemory readingOfFiscalMemory = await db.ReadingOfFiscalMemories.FindAsync(id);
            if (readingOfFiscalMemory == null)
            {
                return HttpNotFound();
            }
            return View(readingOfFiscalMemory);
        }

        // POST: /ReadingOfFiscalMemories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReadingOfFiscalMemory readingOfFiscalMemory = await db.ReadingOfFiscalMemories.FindAsync(id);
            Module module = await db.Modules.FindAsync(readingOfFiscalMemory.ModuleId);
            if(module == null)
            {
                return HttpNotFound();
            }

            //usun odczyt tylko w przypadku jesli pamiec fiskalna jest ostatnim modułem (cały czas aktywna)  
            if(!module.Active)
            {
                ModelState.AddModelError(string.Empty, "Nie można usunąć rekordu");
                return PartialView(readingOfFiscalMemory);
            }

            module.Status = "FISKALNY";
            db.ReadingOfFiscalMemories.Remove(readingOfFiscalMemory);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException/* ex*/)
            {
                ModelState.AddModelError(string.Empty, "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych");
                return PartialView(readingOfFiscalMemory);
            }
            return Json(new { url = Url.Action("Index", "ReadingOfFiscalMemories"), success = true });
        }

        public async Task<ActionResult> GetReportPdf(int id)
        {
            ReadingOfFiscalMemory readerFiscalMemory = await db.ReadingOfFiscalMemories.FindAsync(id);
            if (readerFiscalMemory == null)
            {
                return HttpNotFound();
            }

            XtraReport reportService = new XtraReport();
            string pathToReportService = Server.MapPath("~/Reports/odczyt_page1.repx");
            reportService.LoadLayout(pathToReportService);
            reportService.Parameters["ID"].Value = id;
            reportService.CreateDocument();

            XtraReport reportCustomer = new XtraReport();
            string pathToReportCustomer = Server.MapPath("~/Reports/odczyt_page2.repx");
            reportCustomer.LoadLayout(pathToReportCustomer);
            reportCustomer.Parameters["ID"].Value = id;
            reportCustomer.CreateDocument();

            XtraReport unregisterDevice = new XtraReport();
            string pathToUnregisterDevice = Server.MapPath("~/Reports/wyrejestrowanie_kasy.repx");
            unregisterDevice.LoadLayout(pathToUnregisterDevice);
            unregisterDevice.Parameters["ID"].Value = id;
            unregisterDevice.Parameters["MODULE_ID"].Value = readerFiscalMemory.ModuleId;
            unregisterDevice.CreateDocument();

            XtraReport applicationForReading = new XtraReport();
            string pathToApplicationForReading = Server.MapPath("~/Reports/wniosek_dokonanie_odczytu.repx");
            applicationForReading.LoadLayout(pathToApplicationForReading);
            applicationForReading.Parameters["ID"].Value = id;
            applicationForReading.Parameters["MODULE_ID"].Value = readerFiscalMemory.ModuleId;
            applicationForReading.CreateDocument();

            reportService.Pages.AddRange(reportCustomer.Pages);
            reportService.Pages.AddRange(unregisterDevice.Pages);
            reportService.Pages.AddRange(applicationForReading.Pages);
            reportService.PrintingSystem.ContinuousPageNumbering = false;

            var stream = new MemoryStream();
            reportService.ExportToPdf(stream);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "odczyt.pdf",
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(stream.GetBuffer(), "application/pdf");
        }

        private List<SelectListItem> GetVatOldList()
        {
            return new List<SelectListItem>
                    {
                    new SelectListItem{ Value = "22%", Text="22%"},
                    new SelectListItem{ Value = "7%", Text="7%"},
                    new SelectListItem{ Value = "0%", Text="0%"},
                    new SelectListItem{ Value = "3%", Text="3%"},
                    new SelectListItem{ Value = "zw", Text="zw"}
                    };
        }

        private List<SelectListItem> GetVatNewList()
        {
            return new List<SelectListItem>
                    {
                    new SelectListItem{ Value = "23%", Text="23%"},
                    new SelectListItem{ Value = "8%", Text="8%"},
                    new SelectListItem{ Value = "0%", Text="0%"},
                    new SelectListItem{ Value = "5%", Text="5%"},
                    new SelectListItem{ Value = "zw", Text="zw"}
                    };
        }

        private List<SelectListItem> GetVatZWList()
        {
            return new List<SelectListItem>
                    {
                    new SelectListItem{ Value = "zw", Text="zw"}
                    };
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
