using CGHSCM.DAL;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGHSCM.Controllers
{
    public class AdminController : Controller
    {
        private LogisticContext db = new LogisticContext();

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase inFile)
        {
            if (inFile.ContentLength > 0)
            {
                string fileName = inFile.FileName;
                string fileSavePath = Server.MapPath("~/App_Data/Temp/" + fileName);
                Processes p = new Processes();

                inFile.SaveAs(fileSavePath);

                string[] acceptable_names = { "materials.csv", "outstandings.csv", "costcenters.csv" };
                string selection = Request.Form["selection"];

                if ((!acceptable_names.Contains(fileName)
                    || !acceptable_names.Contains(selection + ".csv"))
                    && selection != "outstandings")
                {
                    // Conditions are as follows -> selection must not be transactions,
                    //    (selection name must match file name, file name must match acceptable names
                    Session["Upload"] = false;
                    Session["Reason"] = "CSV file name inproper";                   
                    return Redirect("index");
                }

                bool success = p.ProcessCSV(fileName, fileSavePath);

                if (System.IO.File.Exists(fileSavePath))
                {
                    Console.WriteLine("File Exists, Delete File");
                    System.IO.File.Delete(fileSavePath);
                    Console.WriteLine("File Deleted");
                }

                if (!success)
                {
                    Session["Upload"] = false;
                    Session["Reason"] = "Errors in server during uploading process";
                    return Redirect("index");
                }

            }

            Session["Upload"] = true;
            Session["Reason"] = "Upload successful";
            return Redirect("index");
        }
    }
}