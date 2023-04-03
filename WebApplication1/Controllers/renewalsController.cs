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
    public class renewalsController : Controller
    {
        private IdCardDataManagerEntities1 db = new IdCardDataManagerEntities1();

        // GET: renewals
        public ActionResult Index()
        {
            return View(db.renewals.ToList());
        }

        // GET: renewals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            renewal renewal = db.renewals.Find(id);
            if (renewal == null)
            {
                return HttpNotFound();
            }
            return View(renewal);
        }

        // GET: renewals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: renewals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,mobile,current_validity,new_validity,customer_amt,dealer_amt,paid,renewal__date")] renewal renewal)
        {
            if (ModelState.IsValid)
            {
                db.renewals.Add(renewal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(renewal);
        }

        // GET: renewals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            renewal renewal = db.renewals.Find(id);
            if (renewal == null)
            {
                return HttpNotFound();
            }
            return View(renewal);
        }

        // POST: renewals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,mobile,current_validity,new_validity,customer_amt,dealer_amt,paid,renewal__date")] renewal renewal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(renewal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(renewal);
        }

        // GET: renewals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            renewal renewal = db.renewals.Find(id);
            if (renewal == null)
            {
                return HttpNotFound();
            }
            return View(renewal);
        }

        // POST: renewals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            renewal renewal = db.renewals.Find(id);
            db.renewals.Remove(renewal);
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
