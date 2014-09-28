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
    public class CustomersSpecialNotesAdminController : Controller
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
        // GET: /CustomersSpecialNotesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<CustomersSpecialNotes> qryCustomersSpecialNotes = null;

            List<CustomersSpecialNotes> CustomersSpecialNotesList = new List<CustomersSpecialNotes>();

            qryCustomersSpecialNotes = db.CustomersSpecialNotes.OrderBy(csn => csn.CustomerId);
            if (qryCustomersSpecialNotes.Count() > 0)
            {
                foreach (var item in qryCustomersSpecialNotes)
                {
                    CustomersSpecialNotesList.Add(item);
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


            var onePageOfData = CustomersSpecialNotesList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(CustomersSpecialNotesList.ToPagedList(pageIndex, pageSize));
            //return View(db.CustomersSpecialNotes.ToList());
        }

        //
        // GET: /CustomersSpecialNotesAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomersSpecialNotes customersspecialnotes = db.CustomersSpecialNotes.Find(id);
            if (customersspecialnotes == null)
            {
                return HttpNotFound();
            }
            return View(customersspecialnotes);
        }

        //
        // GET: /CustomersSpecialNotesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomersSpecialNotesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersSpecialNotes customersspecialnotes)
        {
            if (ModelState.IsValid)
            {
                db.CustomersSpecialNotes.Add(customersspecialnotes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customersspecialnotes);
        }

        //
        // GET: /CustomersSpecialNotesAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomersSpecialNotes customersspecialnotes = db.CustomersSpecialNotes.Find(id);
            if (customersspecialnotes == null)
            {
                return HttpNotFound();
            }
            return View(customersspecialnotes);
        }

        //
        // POST: /CustomersSpecialNotesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersSpecialNotes customersspecialnotes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customersspecialnotes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customersspecialnotes);
        }

        //
        // GET: /CustomersSpecialNotesAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomersSpecialNotes customersspecialnotes = db.CustomersSpecialNotes.Find(id);
            if (customersspecialnotes == null)
            {
                return HttpNotFound();
            }
            return View(customersspecialnotes);
        }

        //
        // POST: /CustomersSpecialNotesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomersSpecialNotes customersspecialnotes = db.CustomersSpecialNotes.Find(id);
            db.CustomersSpecialNotes.Remove(customersspecialnotes);
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