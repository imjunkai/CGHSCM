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
    public class CostCentersController : Controller
    {
        private LogisticContext db = new LogisticContext();

        // GET: CostCenters               
        public async Task<ActionResult> Index(string id, string searchString)
        {
            //id here refers to sort order
            ViewBag.NameSortParam = String.IsNullOrEmpty(id) ? "name_desc" : "";
            ViewBag.IDSortParam = id == "id" ? "id_desc" : "id";

            var centers = from c in db.CostCenters
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                centers = centers.Where(c => c.CostCenterName.Contains(searchString)
                                        || c.CostCenterID.Contains(searchString));
            }

            switch (id)
            {
                case "name_desc":
                    centers = centers.OrderByDescending(x => x.CostCenterName);
                    break;
                case "id":
                    centers = centers.OrderBy(x => x.CostCenterID);
                    break;
                case "id_desc":
                    centers = centers.OrderByDescending(x => x.CostCenterID);
                    break;
                default:
                    centers = centers.OrderBy(x => x.CostCenterName);
                    break;
            }

            return View(await centers.ToListAsync());

        }

        // GET: CostCenters/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostCenter costCenter = await db.CostCenters.FindAsync(id);
            if (costCenter == null)
            {
                return HttpNotFound();
            }
            return View(costCenter);
        }

        // GET: CostCenters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CostCenters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CostCenterID,CostCenterName")] CostCenter costCenter)
        {
            if (ModelState.IsValid)
            {
                db.CostCenters.Add(costCenter);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(costCenter);
        }

        // GET: CostCenters/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostCenter costCenter = await db.CostCenters.FindAsync(id);
            if (costCenter == null)
            {
                return HttpNotFound();
            }
            return View(costCenter);
        }

        // POST: CostCenters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CostCenterID,CostCenterName")] CostCenter costCenter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costCenter).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(costCenter);
        }

        // GET: CostCenters/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostCenter costCenter = await db.CostCenters.FindAsync(id);
            if (costCenter == null)
            {
                return HttpNotFound();
            }
            return View(costCenter);
        }

        // POST: CostCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            CostCenter costCenter = await db.CostCenters.FindAsync(id);
            db.CostCenters.Remove(costCenter);
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
