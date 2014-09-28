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
    public class VendorsSpecialNotesAdminController : Controller
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
        // GET: /VendorsSpecialNotesAdmin/

        public ActionResult Index(int? page)
        {
            int pageIndex = 0;
            int pageSize = PageSize;
            IQueryable<VendorsSpecialNotes> qryVendorsNote = null;

            List<VendorsSpecialNotes> VendorsSpecialNotesList = new List<VendorsSpecialNotes>();

            qryVendorsNote = db.VendorsSpecialNotes.OrderBy(vsn => vsn.VendorId);
            if (qryVendorsNote.Count() > 0)
            {
                foreach (var item in qryVendorsNote)
                {
                    VendorsSpecialNotesList.Add(item);
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


            var onePageOfData = VendorsSpecialNotesList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return View(VendorsSpecialNotesList.ToPagedList(pageIndex, pageSize));
            //return View(db.VendorsSpecialNotes.ToList());
        }

        //
        // GET: /VendorsSpecialNotesAdmin/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorsSpecialNotes vendorsspecialnotes = db.VendorsSpecialNotes.Find(id);
            if (vendorsspecialnotes == null)
            {
                return HttpNotFound();
            }
            return View(vendorsspecialnotes);
        }

        //
        // GET: /VendorsSpecialNotesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorsSpecialNotesAdmin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorsSpecialNotes vendorsspecialnotes)
        {
            if (ModelState.IsValid)
            {
                db.VendorsSpecialNotes.Add(vendorsspecialnotes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendorsspecialnotes);
        }

        //
        // GET: /VendorsSpecialNotesAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorsSpecialNotes vendorsspecialnotes = db.VendorsSpecialNotes.Find(id);
            if (vendorsspecialnotes == null)
            {
                return HttpNotFound();
            }
            return View(vendorsspecialnotes);
        }

        //
        // POST: /VendorsSpecialNotesAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorsSpecialNotes vendorsspecialnotes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorsspecialnotes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendorsspecialnotes);
        }

        //
        // GET: /VendorsSpecialNotesAdmin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorsSpecialNotes vendorsspecialnotes = db.VendorsSpecialNotes.Find(id);
            if (vendorsspecialnotes == null)
            {
                return HttpNotFound();
            }
            return View(vendorsspecialnotes);
        }

        //
        // POST: /VendorsSpecialNotesAdmin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorsSpecialNotes vendorsspecialnotes = db.VendorsSpecialNotes.Find(id);
            db.VendorsSpecialNotes.Remove(vendorsspecialnotes);
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