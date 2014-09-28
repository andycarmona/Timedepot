using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;
using PagedList;

namespace TimelyDepotMVC.Controllers.Admin
{
    public class PRICEAdminController : Controller
    {
        private TimelyDepotContext db = new TimelyDepotContext();

        int _pageIndex = 0;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        int _pageSize = 25;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        
        //
        // GET: /PRICEAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<PRICE> qryPr = null;

            List<PRICE> priceList = new List<PRICE>();

            qryPr = db.PRICEs.OrderBy(pr => pr.Item);
            if (qryPr.Count() > 0)
            {
                foreach (var item in qryPr)
                {
                    priceList.Add(item);
                }
            }

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = priceList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(priceList.ToPagedList(pageIndex, pageSize));
            //return View(db.PRICEs.ToList());
        }

        //
        // GET: /PRICEAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            PRICE price = db.PRICEs.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        //
        // GET: /PRICEAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PRICEAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PRICE price)
        {
            if (ModelState.IsValid)
            {
                db.PRICEs.Add(price);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(price);
        }

        //
        // GET: /PRICEAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PRICE price = db.PRICEs.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        //
        // POST: /PRICEAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PRICE price)
        {
            if (ModelState.IsValid)
            {
                db.Entry(price).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(price);
        }

        //
        // GET: /PRICEAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PRICE price = db.PRICEs.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        //
        // POST: /PRICEAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PRICE price = db.PRICEs.Find(id);
            db.PRICEs.Remove(price);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}