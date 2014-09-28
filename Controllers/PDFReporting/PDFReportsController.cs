using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ReportManagement;
using TimelyDepotMVC.DAL;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.Models;

namespace TimelyDepotMVC.Controllers.PDFReporting
{
    public class PDFReportsController : PdfViewController
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        //
        // GET: /PDFReports/

        public ActionResult Index()
        {
            List<Clients> clientsList = new List<Clients>();
            Clients client = null;

            IQueryable<CustomersContactAddress> qryContactAddress = db.CustomersContactAddresses.OrderBy(ctad => ctad.CompanyName);
            if (qryContactAddress.Count() > 0)
            {
                foreach (var item in qryContactAddress)
                {
                    client = new Clients();
                    client.Name = item.CompanyName;
                    client.Address = string.Format("{0} {1}",item.Address, item.Note);
                    client.Place = string.Format("{0} {1}", item.City, item.Country);
                    clientsList.Add(client);
                }
            }

            //return View(clientsList);
            return ViewPdf("Clients List", "Index", clientsList);
        }

    }
}
