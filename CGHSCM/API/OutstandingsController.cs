using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using CGHSCM.DAL;
using CGHSCM.Models;


namespace CGHSCM.API
{
    public class OutstandingsController : ApiController
    {
        private LogisticContext db = new LogisticContext();

        // GET: api/Outstandings
        public IQueryable<Outstanding> GetOutstandings()
        {
            
            Task.Factory.StartNew(new Processes().UpdateOutstanding);

            var query = from o in db.Outstandings
                        where o.IsDone == false
                        select o;

            return query;
        }

        [Route("api/outstandings/{ward}")]
        public IQueryable<Outstanding> GetOutstandings(string ward)
        {
            ward = ward.Replace('_', ' ').ToUpper();

            Task.Factory.StartNew(new Processes().UpdateOutstanding);

            var query = from o in db.Outstandings
                        where
                            o.CostCenterName == ward &&
                            (o.IsDone == false || (o.IsDone == true && o.TimeUnlock > DateTime.Now))
                        select o;

            return query;
        }

        // POST: api/Outstandings        
        public async Task<IHttpActionResult> PostOutstanding(List<Outstanding> outstandings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await Task.Factory.StartNew(() =>
            {
                new Processes().UpdateOutstanding(db, outstandings);
            });
            
            return Json("'info': 'sent update request'");

        }       
        public async Task<IHttpActionResult> DeleteOutstanding(Outstanding outstanding)
        {            
            db.Outstandings.Remove(outstanding);
            await Task.Factory.StartNew(() =>
            {
                new Processes().DeleteOutstanding(outstanding);
            });
            return Json("'Info': 'Deleted Item'");
        }
    }
}