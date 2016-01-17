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
    public class OutstandingsController : Controller
    {
        private LogisticContext db = new LogisticContext();

        // GET: Outstandings
        public async Task<ActionResult> Index()
        {
            return View(await db.Outstandings.ToListAsync());
        }

        // GET: Outstandings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outstanding outstanding = await db.Outstandings.FindAsync(id);
            if (outstanding == null)
            {
                return HttpNotFound();
            }
            return View(outstanding);
        }

        // GET: Outstandings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Outstandings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,MaterialID,Description,TopUp,UOM,CostCenterID,CostCenterName,IsDone,TimeRequested,TimeDone,TimeUnlock")] Outstanding outstanding)
        {
            if (ModelState.IsValid)
            {
                db.Outstandings.Add(outstanding);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(outstanding);
        }

        // GET: Outstandings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outstanding outstanding = await db.Outstandings.FindAsync(id);
            if (outstanding == null)
            {
                return HttpNotFound();
            }
            return View(outstanding);
        }

        // POST: Outstandings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,MaterialID,Description,TopUp,UOM,CostCenterID,CostCenterName,IsDone,TimeRequested,TimeDone,TimeUnlock")] Outstanding outstanding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outstanding).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(outstanding);
        }

        // GET: Outstandings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outstanding outstanding = await db.Outstandings.FindAsync(id);
            if (outstanding == null)
            {
                return HttpNotFound();
            }
            return View(outstanding);
        }

        // POST: Outstandings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Outstanding outstanding = await db.Outstandings.FindAsync(id);
            db.Outstandings.Remove(outstanding);
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
