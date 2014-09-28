using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using TimelyDepotMVC.DAL;
using TimelyDepotMVC.Models.Admin;
using System.Data;

namespace TimelyDepotMVC.Controllers.Admin
{
    using System.Data.Entity;

    public class TestsAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();


        //
        // GET: /TestsAdmin/ImportInventory
        public ActionResult ImportInventory()
        {
            int nPos = 0;
            string szItemId = "";
            string szError = "";
            string szMsg = "";
            string szFilePath = "~/Content/ImportData/Inventory.csv";
            string szFilePathHlp = "";
            string[] szHeaders = null;
            string[] szRow = null;

            ITEM item = null;

            szFilePathHlp = Server.MapPath(szFilePath);

            //Import the excel data
            if (System.IO.File.Exists(szFilePathHlp))
            {
                //Read the data
                using (StreamReader sr = System.IO.File.OpenText(szFilePathHlp))
                {
                    string input;
                    while ((input = sr.ReadLine()) != null)
                    {
                        if (nPos == 0)
                        {
                            szHeaders = input.Split(',');
                            nPos++;
                        }
                        else
                        {
                            szRow = input.Split(',');
                            if (szItemId != szRow[0])
                            {
                                szItemId = szRow[0];
                                UpdateInventory(szHeaders, szRow, db, item, nPos, ref szError);
                                if (!string.IsNullOrEmpty(szError))
                                {
                                    break;
                                }

                            }
                            nPos++;
                            //if (nPos > 20)
                            //{
                            //    break;
                            //}
                        }
                    }

                }
            }


            szMsg = string.Format("Imported excel data. (Rows: {0})", nPos.ToString());
            if (!string.IsNullOrEmpty(szError))
            {
                szMsg = string.Format("Imported excel data. ({0})", szError);

            }
            TempData["ImportMessage"] = szMsg;

            return RedirectToAction("Index");
        }

        private void UpdateInventory(string[] szHeaders, string[] szRow, TimelyDepotContext db, ITEM item, int nPos, ref string szError)
        {

            //Asume
            // 0 = ItemId
            // 1 = PROD_CD
            // 2 = UnitperCase
            // 3 = CaseWeight
            // 4 = UnitWeight
            // 5 = CaseDimensionL
            // 6 = CaseDimensionW
            // 7 = CaseDimensionH

            string szItemId = "";
            szError = "";

            try
            {
                szItemId = szRow[0];
                item = db.ITEMs.Where(itm => itm.ItemID == szItemId).FirstOrDefault<ITEM>();
                if (item != null)
                {
                    item.UnitPerCase = szRow[2];
                    item.CaseWeight = szRow[3];
                    item.UnitWeight = szRow[4];
                    item.CaseDimensionL = szRow[5];
                    item.CaseDimensionW = szRow[6];
                    item.CaseDimensionH = szRow[7];
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch (Exception err)
            {
                szError = string.Format("Row {0}: {1} {2}", nPos, err.Message, err.StackTrace);
            }
        }

        //
        // GET: /TestsAdmin/
        public ActionResult Index()
        {
            if (TempData["ImportMessage"] != null)
            {
                ViewBag.ImportMessage = TempData["ImportMessage"].ToString();
            }

            return View();
        }

    }
}
