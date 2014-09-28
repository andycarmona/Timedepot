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
    public class TradeAdminController : Controller
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
        // GET: /TradeAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<Trade> qryTrade = null;

            List<Trade> TradeList = new List<Trade>();

            qryTrade = db.Trades.OrderBy(trd => trd.TradeName);
            if (qryTrade.Count() > 0)
            {
                foreach (var item in qryTrade)
                {
                    TradeList.Add(item);
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


            var onePageOfData = TradeList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(TradeList.ToPagedList(pageIndex, pageSize));
            //return View(db.Trades.ToList());
        }

        //
        // GET: /TradeAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        //
        // GET: /TradeAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TradeAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Trade trade)
        {
            if (ModelState.IsValid)
            {
                db.Trades.Add(trade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trade);
        }

        //
        // GET: /TradeAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        //
        // POST: /TradeAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Trade trade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trade);
        }

        //
        // GET: /TradeAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        //
        // POST: /TradeAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trade trade = db.Trades.Find(id);
            db.Trades.Remove(trade);
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