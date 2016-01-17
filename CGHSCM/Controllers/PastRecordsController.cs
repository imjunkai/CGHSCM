using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CGHSCM.DAL;
using CGHSCM.Models;

namespace CGHSCM.Controllers
{
    public class PastRecordsController : Controller
    {
        private LogisticContext db = new LogisticContext();

        // GET: PastRecords
        public async Task<ActionResult> Index()
        {
            return View(await db.PastRecords.ToListAsync());
        }

        // GET: PastRecords/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PastRecord pastRecord = await db.PastRecords.FindAsync(id);
            if (pastRecord == null)
            {
                return HttpNotFound();
            }
            return View(pastRecord);
        }

        // GET: PastRecords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PastRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,MaterialID,Description,TopUp,UOM,CostCenterID,CostCenterName,TimeRequested,TimeDone")] PastRecord pastRecord)
        {
            if (ModelState.IsValid)
            {
                db.PastRecords.Add(pastRecord);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pastRecord);
        }

        // GET: PastRecords/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PastRecord pastRecord = await db.PastRecords.FindAsync(id);
            if (pastRecord == null)
            {
                return HttpNotFound();
            }
            return View(pastRecord);
        }

        // POST: PastRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,MaterialID,Description,TopUp,UOM,CostCenterID,CostCenterName,TimeRequested,TimeDone")] PastRecord pastRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pastRecord).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pastRecord);
        }

        // GET: PastRecords/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PastRecord pastRecord = await db.PastRecords.FindAsync(id);
            if (pastRecord == null)
            {
                return HttpNotFound();
            }
            return View(pastRecord);
        }

        // POST: PastRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PastRecord pastRecord = await db.PastRecords.FindAsync(id);
            db.PastRecords.Remove(pastRecord);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
