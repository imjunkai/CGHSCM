using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CGHSCM.DAL;
using CGHSCM.Models;
using OfficeOpenXml;
using System.IO;

namespace CGHSCM.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Excel(string id)
        {
            string filename;
            FileInfo newFile;
            LogisticContext db = new LogisticContext();

            switch (id)
            {
                case "recent":
                    filename = Path.Combine(Server.MapPath("~/App_Data/Temp"),
                        string.Format("{0}.xlsx", Guid.NewGuid().ToString()));
                    newFile = new FileInfo(filename);

                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Recent Items");
                        string[] headers = { "Material ID", "Description", "Top Up", "UOM", "Cost Center ID", "Cost Center", "Time Requested", "Time Done" };

                        int row = 1;
                        for (int i = 1; i <= headers.Length; i++)
                        {
                            sheet.Cells[row, i].Value = headers[i - 1];
                        }
                        row++;

                        DateTime compare = DateTime.Now.AddHours(-1);
                        var query = from o in db.Outstandings
                                    where o.IsDone == true && o.TimeDone >= compare
                                    select new
                                    {
                                        o.MaterialID,
                                        o.Description,
                                        o.TopUp,
                                        o.UOM,
                                        o.CostCenterID,
                                        o.CostCenterName,
                                        o.TimeRequested,
                                        o.TimeDone
                                    };
                        foreach (var o in query.ToList())
                        {
                            sheet.Cells[row, 1].Value = o.MaterialID;
                            sheet.Cells[row, 2].Value = o.Description;
                            sheet.Cells[row, 3].Value = o.TopUp;
                            sheet.Cells[row, 4].Value = o.UOM;
                            sheet.Cells[row, 5].Value = o.CostCenterID;
                            sheet.Cells[row, 6].Value = o.CostCenterName;
                            sheet.Cells[row, 7].Value = o.TimeRequested.ToString();
                            sheet.Cells[row, 8].Value = o.TimeDone.Value.ToString();
                            row++;
                        }
                        package.Save();
                    }

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=report.xlsx");
                    Response.TransmitFile(newFile.FullName);
                    Response.End();
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                    }

                    break;
                case "get_all":
                    filename = Path.Combine(Server.MapPath("~/App_Data/Temp"),
                        string.Format("{0}.xlsx", Guid.NewGuid().ToString()));
                    newFile = new FileInfo(filename);

                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Recent Items");
                        string[] headers = { "Material ID", "Description", "Top Up", "UOM", "Cost Center ID", "Cost Center", "Time Requested", "Time Done" };

                        int row = 1;
                        for (int i = 1; i <= headers.Length; i++)
                        {
                            sheet.Cells[row, i].Value = headers[i - 1];
                        }
                        row++;

                        DateTime compare = DateTime.Now.AddHours(-1);
                        var query = from p in db.PastRecords                                    
                                    select new
                                    {
                                        p.MaterialID,
                                        p.Description,
                                        p.TopUp,
                                        p.UOM,
                                        p.CostCenterID,
                                        p.CostCenterName,
                                        p.TimeRequested,
                                        p.TimeDone
                                    };
                        foreach (var p in query.ToList())
                        {
                            sheet.Cells[row, 1].Value = p.MaterialID;
                            sheet.Cells[row, 2].Value = p.Description;
                            sheet.Cells[row, 3].Value = p.TopUp;
                            sheet.Cells[row, 4].Value = p.UOM;
                            sheet.Cells[row, 5].Value = p.CostCenterID;
                            sheet.Cells[row, 6].Value = p.CostCenterName;
                            sheet.Cells[row, 7].Value = p.TimeRequested.ToString();
                            sheet.Cells[row, 8].Value = p.TimeDone.ToString();
                            row++;
                        }
                        package.Save();
                    }

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=AllRecords.xlsx");
                    Response.TransmitFile(newFile.FullName);
                    Response.End();
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                    }
                    break;
                default:
                    Session["Download"] = false;
                    Session["arg"] = id;
                    return Redirect("~");
            }

            Session["Download"] = true;
            return Redirect("~");
        }

    }
}