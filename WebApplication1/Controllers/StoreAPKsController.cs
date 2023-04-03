using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Controllers
{
    public class StoreAPKsController : Controller
    {
        private IdCardDataManagerEntities1 db = new IdCardDataManagerEntities1();

        // GET: StoreAPKs
        public ActionResult Index()
        {
            return View(db.StoreAPKs.ToList());
        }

        // GET: StoreAPKs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreAPK storeAPK = db.StoreAPKs.Find(id);
            if (storeAPK == null)
            {
                return HttpNotFound();
            }
            return View(storeAPK);
        }

        // GET: StoreAPKs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreAPKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,brand")] StoreAPK storeAPK)
        {
            if (ModelState.IsValid)
            {
                db.StoreAPKs.Add(storeAPK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storeAPK);
        }

        // GET: StoreAPKs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreAPK storeAPK = db.StoreAPKs.Find(id);
            if (storeAPK == null)
            {
                return HttpNotFound();
            }
            return View(storeAPK);
        }

        // POST: StoreAPKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,brand")] StoreAPK storeAPK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storeAPK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storeAPK);
        }

        // GET: StoreAPKs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreAPK storeAPK = db.StoreAPKs.Find(id);
            if (storeAPK == null)
            {
                return HttpNotFound();
            }
            return View(storeAPK);
        }

        // POST: StoreAPKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreAPK storeAPK = db.StoreAPKs.Find(id);
            db.StoreAPKs.Remove(storeAPK);
            db.SaveChanges();
            return RedirectToAction("Index");
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
