using WebApplication1.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using static WebApplication1.Models.Class1;

namespace WebApplication1.Controllers
{
    [AutorizeUser]
    public class UsersController : Controller
    {
        private IdCardDataManagerEntities1 db = new IdCardDataManagerEntities1();

        // GET: Users
        [AutorizeUser]
        public ActionResult Index()
        {
            ViewBag.userlist = db.Users.ToList();
            return View(db.Customers.ToList().OrderByDescending(X=>X.CustomerId));

        }

        // GET: Users/Details/5

        [AutorizeUser]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AutorizeUser]
        public ActionResult ChangePassword(WebApplication1.Models.ChangePasswordViewModel ch)
        {
            var q = db.Users.FirstOrDefault();
            if(q.Password==ch.OldPassword)
            {
                if(ch.NewPassword==ch.ConfirmPassword)
                {
                    q.Password = ch.NewPassword;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errormsg"] = "New Password and Conform Password Not Match..!";
                    return View(ch);
                }
               
            }
            else
            {
                TempData["errormsg"] = "Old Password Invalid..!";
                return View(ch);
            }


            return View();
        }
        [HttpPost]
        public ActionResult cpass(int id, string pass)
        {
            var q = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            q.Password = pass;
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


        [AutorizeUser]
        public ActionResult Create()
        {
            Customer cs = new Customer();
            cs.EnteredOnDate = DateTime.Today;
            cs.ValidFrom = DateTime.Today;
            cs.ValidTill = DateTime.Today.AddYears(1);
            var q = db.Dealers.ToList();
            ViewBag.dealerid = new SelectList(q, "id", "name");
            return View(cs);
        }


        [HttpPost]
      
        [AutorizeUser]
        public ActionResult Create([Bind(Include = "CustomerId,CustName,IsValid,ValidTill,EnteredOnDate,ValidFrom,CustAddress,ContactNo,Email,CustCode,UpdatedOn,UpdatedBy,password,dealerid,dealerprice,customerprice")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.UpdatedOn = DateTime.Today;
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        [AutorizeUser]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var q = db.Dealers.Where(x=>x.id==customer.dealerid).ToList();
            ViewBag.dealerid = new SelectList(q, "id", "name");
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult Edit([Bind(Include = "CustomerId,CustName,IsValid,ValidTill,EnteredOnDate,ValidFrom,CustAddress,ContactNo,Email,CustCode,UpdatedOn,UpdatedBy,password,dealerid,dealerprice,customerprice")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                var q = db.Dealers.Where(x => x.id == customer.dealerid).ToList();
                ViewBag.dealerid = new SelectList(q, "id", "name");
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [AutorizeUser]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var q = db.Dealers.Where(x => x.id == customer.dealerid).ToList();
            ViewBag.dealerid = new SelectList(q, "id", "name");
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            var q = db.Dealers.Where(x => x.id == customer.dealerid).ToList();
            ViewBag.dealerid = new SelectList(q, "id", "name");
            return RedirectToAction("Index");
        }

        public ActionResult Renew(int id)
        {
            var q = db.Customers.Where(x => x.CustomerId == id).FirstOrDefault();
            renewal rn = new renewal();
            rn.name = q.CustName;
            rn.mobile = Convert.ToInt64(q.ContactNo);
            rn.current_validity = q.ValidTill;
            rn.new_validity =q.ValidTill.AddYears(1);
            rn.customer_amt = q.customerprice;
            rn.dealer_amt = q.dealerprice;
            rn.paid = false;
            rn.renewal__date = DateTime.Today;
            rn.id = id;
            return View(rn);
        }

        [HttpPost]
        public ActionResult Renew(renewal renewal)
        {
            var q = db.Customers.Where(x => x.CustomerId == renewal.id).FirstOrDefault();
            q.ValidTill =Convert.ToDateTime( renewal.new_validity);
            q.customerprice = renewal.customer_amt;
            q.dealerprice = renewal.dealer_amt;
            db.SaveChanges();
            renewal rn = new renewal();
            rn.name = q.CustName;
            rn.mobile =Convert.ToInt64( q.ContactNo);
            rn.current_validity = renewal.current_validity;
            rn.new_validity = renewal.new_validity;
            rn.customer_amt =renewal.customer_amt;
            rn.dealer_amt = renewal.dealer_amt;
            rn.paid = renewal.paid;
            rn.renewal__date = DateTime.Today;
            db.renewals.Add(rn);
            db.SaveChanges();
            return RedirectToAction("Index");

        }




    }
}
