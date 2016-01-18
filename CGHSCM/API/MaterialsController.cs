using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CGHSCM.DAL;
using CGHSCM.Models;

namespace CGHSCM.API
{
    public class MaterialsController : ApiController
    {
        private LogisticContext db = new LogisticContext();

        // GET: api/Materials
        public IQueryable<Material> GetMaterials()
        {
            return db.Materials;
        }

        // GET: api/Materials/5
        [ResponseType(typeof(Material))]
        public async Task<IHttpActionResult> GetMaterial(string id)
        {
            Material material = await db.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        // PUT: api/Materials/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMaterial(string id, Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != material.MaterialID)
            {
                return BadRequest();
            }

            db.Entry(material).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Materials
        [ResponseType(typeof(Material))]
        public async Task<IHttpActionResult> PostMaterial(Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Materials.Add(material);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MaterialExists(material.MaterialID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = material.MaterialID }, material);
        }

        // DELETE: api/Materials/5
        [ResponseType(typeof(Material))]
        public async Task<IHttpActionResult> DeleteMaterial(string id)
        {
            Material material = await db.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            db.Materials.Remove(material);
            await db.SaveChangesAsync();

            return Ok(material);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MaterialExists(string id)
        {
            return db.Materials.Count(e => e.MaterialID == id) > 0;
        }
    }
}