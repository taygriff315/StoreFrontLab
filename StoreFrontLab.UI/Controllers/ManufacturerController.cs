using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFrontLab.Data.EF;
using System.Net;
using System.Data.Entity;

namespace StoreFrontLab.UI.Controllers
{
    public class ManufacturerController : Controller
    {

        private StoreFrontEntities db = new StoreFrontEntities();
        // GET: Manufacturer
        public ActionResult Index()
        {


            return View(db.Manufacturers.ToList());

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ManufacturerDelete(int id)
        {
            Manufacturer manufacturer = db.Manufacturers.Find(id);

            db.Manufacturers.Remove(manufacturer);
            db.SaveChanges();

            string confirmMessage = string.Format("Deleted Manufacturer '{0}' from the database", manufacturer.ManufacturerName);
            return Json(new { id = id, message = confirmMessage });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ManufacturerCreate(Manufacturer manufacturer)
        {
            db.Manufacturers.Add(manufacturer);
            db.SaveChanges();
            return Json(manufacturer);
        }

        [HttpGet]
        public PartialViewResult ManufacturerDetails(int id)
        {
            Manufacturer manufacturer = db.Manufacturers.Find(id);
            return PartialView(manufacturer);
        }

        [HttpGet]
        public PartialViewResult ManufacturerEdit(int id)
        {
            Manufacturer manufacturer = db.Manufacturers.Find(id);
            return PartialView(manufacturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ManufacturerEdit(Manufacturer manufacturer)
        {
            db.Entry(manufacturer).State = EntityState.Modified;
            db.SaveChanges();
            return Json(manufacturer);
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