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
using Inspinia_MVC5_SeedProject.ViewModels.InterimReviews;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Data.Entity.Infrastructure;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class InterimReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /InterimReviews/
        public async Task<ActionResult> Index()
        {
            var interimReviews = db.Database.SqlQuery<InterimReviewViewModel>(
                   @"SELECT InterimReviewId, DeviceId, DateOfReview, NextReview, City, Comments, Name as 'DeviceName',
					SerialNumber, UniqueNumber, ServiceMan, ModuleId, DATEDIFF(day, NextReview, GETDATE()) as Dpt
					FROM
					(SELECT i.InterimReviewId, i.ModuleId, d.DeviceId, df.Name, i.City, d.SerialNumber,
					m.UniqueNumber,i.DateOfReview,i.NextReview,
					c.Phone, i.Comments, i.ServiceMan
					FROM Devices d
					inner join Modules m on m.DeviceId = d.DeviceId
					inner join DevicesFolders df on df.DevicesFolderId = d.DevicesFolderId
					INNER JOIN InterimReviews i on m.ModuleId = i.ModuleId
					INNER JOIN ( SELECT  ModuleId, MAX(DateOfReview) Max_Date
								FROM    InterimReviews
								GROUP   BY ModuleId
								) n ON  i.ModuleId = n.ModuleId AND i.DateOfReview = n.Max_date
					inner join FiscalizationToModules fm on fm.ModuleId = m.ModuleId
					inner join Fiscalizations f on f.FiscalizationId = fm.FiscalizationId
					inner join Customers c on c.CustomerId = f.CustomerId
					inner join Addresses ad on ad.AddressId = fm.AddressId
					where(m.Active = 1)) t
					order by t.NextReview");

            return View(await interimReviews.ToListAsync());
        }

        // GET: /InterimReviews/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InterimReview interimReview = await db.InterimReviews.FindAsync(id);
            if (interimReview == null)
            {
                return HttpNotFound();
            }

            return View(interimReview);
        }

        // GET: /InterimReviews/Create
        public ActionResult Create(int? deviceId)
        {
            var review = (from d in db.Devices
                          join m in db.Modules on d.DeviceId equals m.DeviceId
                          join df in db.DevicesFolders on d.DevicesFolderId equals df.DevicesFolderId
                          where ((m.Active == true) && (d.DeviceId == deviceId))
                          select new InterimReviewViewModel()
                          {
                              DeviceId = d.DeviceId,
                              DeviceName = df.Name,
                              SerialNumber = d.SerialNumber,
                              UniqueNumber = m.UniqueNumber
                          }).SingleOrDefault();

            if (review == null)
            {
                return HttpNotFound();
            }

            Device device = db.Devices.SingleOrDefault(d => d.DeviceId == review.DeviceId);
            if (device != null)
                review.NextReview = DateTime.Now.AddMonths(device.ReviewInterval).Date;

            review.DateOfReview = DateTime.Now.Date;

            ViewBag.DeviceId = new SelectList(db.Devices, "DeviceId", "SerialNumber");
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "UniqueNumber");

            return View(review);
        }

        // POST: /InterimReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="InterimReviewId,SerialNumber,UniqueNumber,DateOfReview,NextReview,Comments,City,ServiceMan,DeviceId,ModuleId")] InterimReviewViewModel interimReview)
        {
            if (ModelState.IsValid)
            {
                Module module = db.Modules.Where(d => d.DeviceId == interimReview.DeviceId).Where(m => m.Active == true).FirstOrDefault();
                if (module == null)
                {
                    return HttpNotFound();
                }

                var device = db.Devices.Find(interimReview.DeviceId);
                if (device != null)
                {
                    device.InterimReviewDate = interimReview.NextReview;
                    db.Entry(device).State = EntityState.Modified;
                }

                InterimReview newInterimReview = new InterimReview();

                newInterimReview.City = interimReview.City;
                newInterimReview.Comments = interimReview.Comments;
                newInterimReview.DateOfReview = interimReview.DateOfReview;
                newInterimReview.DeviceId = interimReview.DeviceId;
                newInterimReview.NextReview = interimReview.NextReview;
                newInterimReview.ServiceMan = interimReview.ServiceMan;

                newInterimReview.ModuleId = module.ModuleId;
                db.InterimReviews.Add(newInterimReview);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(interimReview);
        }

        // GET: /InterimReviews/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InterimReview interimReview = await db.InterimReviews.FindAsync(id);
            if (interimReview == null)
            {
                return HttpNotFound();
            }

            var editedInterimReview = (from d in db.Devices
                                       join m in db.Modules on d.DeviceId equals m.DeviceId
                                       join df in db.DevicesFolders on d.DevicesFolderId equals df.DevicesFolderId
                                       join i in db.InterimReviews on m.ModuleId equals i.ModuleId
                                       where i.InterimReviewId == interimReview.InterimReviewId
                                       select new InterimReviewViewModel()
                                       {
                                           InterimReviewId = i.InterimReviewId,
                                           DeviceId = d.DeviceId,
                                           DeviceName = df.Name,
                                           SerialNumber = d.SerialNumber,
                                           UniqueNumber = m.UniqueNumber,
                                           NextReview = i.NextReview,
                                           DateOfReview = i.DateOfReview,
                                           City = i.City,
                                           Comments = i.Comments,
                                           ServiceMan = i.ServiceMan,
                                       }).SingleOrDefaultAsync();

            return View(await editedInterimReview);
        }

        // POST: /InterimReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include= "InterimReviewId,SerialNumber,UniqueNumber,DateOfReview,NextReview,Comments,City,ServiceMan,DeviceId,ModuleId")] InterimReviewViewModel interimReview)
        {
            if (ModelState.IsValid)
            {
                InterimReview interimReviewDb = await db.InterimReviews.FindAsync(interimReview.InterimReviewId);
                if (interimReviewDb == null)
                {
                    return HttpNotFound();
                }

                interimReviewDb.DateOfReview = interimReview.DateOfReview;
                interimReviewDb.NextReview = interimReview.NextReview;
                interimReviewDb.Comments = interimReview.Comments;
                interimReviewDb.City = interimReview.City;
                interimReviewDb.ServiceMan = interimReview.ServiceMan;

                db.Entry(interimReviewDb).State = EntityState.Modified;

                var device = db.Devices.Find(interimReview.DeviceId);
                if (device != null)
                {
                    device.InterimReviewDate = interimReview.NextReview;
                    db.Entry(device).State = EntityState.Modified;
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(interimReview);
        }

        // GET: /InterimReviews/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterimReview interimReview = await db.InterimReviews.FindAsync(id);
            if (interimReview == null)
            {
                return HttpNotFound();
            }
            return PartialView(interimReview);

        }

        // POST: /InterimReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InterimReview interimReview = await db.InterimReviews.FindAsync(id);
            db.InterimReviews.Remove(interimReview);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException /*ex*/)
            {
                string errorMessage = "";
                errorMessage = "Nie można usunąć rekordu, ponieważ jest powiązany z innymi rekordami w bazie danych";

                ModelState.AddModelError(String.Empty, errorMessage);
                return PartialView(interimReview);
            }
            return Json(new { url = Url.Action("Index", "InterimReviews"), success = true });
        }

        public async Task<ActionResult> GetReportPdf(int id)
        {
            InterimReview interimReview = await db.InterimReviews.FindAsync(id);
            if (interimReview == null)
            {
                return HttpNotFound();
            }

            XtraReport report = new XtraReport();

            string path = Server.MapPath("~/Reports/przeglad.repx");
            report.LoadLayout(path);

            report.Parameters["INTERIMREVIEW_ID"].Value = id;

            report.CreateDocument();

            var stream = new MemoryStream();
            report.ExportToPdf(stream);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "przegląd.pdf",
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(stream.GetBuffer(), "application/pdf");
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
