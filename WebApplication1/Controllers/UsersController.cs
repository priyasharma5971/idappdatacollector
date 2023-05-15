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
using System.Globalization;

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
            ViewBag.schoollist = db.Schools.ToList();
            ViewBag.studentlist = db.Students.ToList();
            return View(db.Customers.ToList().OrderByDescending(X=>X.CustomerId));



          
            //var a = (from m in db.Schools.ToList()
            //         join c in db.Customers on m.CustomerId equals c.CustomerId
            //         join s in db.Students on m.SchoolId equals s.StSchool_SchoolId
            //         select new Customer
            //         {
            //             size = s.Size,
            //            CustomerId=m.CustomerId,
            //         }).ToList();


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
            Models.Customer cs = new Models.Customer();
            cs.EnteredOnDate = DateTime.Today.ToString("dd-MM-yyyy");
            cs.ValidFrom = DateTime.Today.ToString("dd-MM-yyyy"); 
            cs.ValidTill = DateTime.Today.AddYears(1).ToString("dd-MM-yyyy");
            var q = db.Dealers.ToList();
            ViewBag.dealerid = new SelectList(q, "id", "name");
            return View(cs);
        }


        [HttpPost]
      
        [AutorizeUser]
        public ActionResult Create(Models.Customer customer)
        {
            string data = "";
            data = customer.ValidFrom.ToString() + " " + customer.ValidTill.ToString();
            DateTime vf, vt;
            
                if (ModelState.IsValid)
                {
                    Customer CS = new Customer();
                    CS.ValidFrom = ConvertToDate(customer.ValidFrom);
                    CS.ValidTill = ConvertToDate(customer.ValidTill);
                    CS.EnteredOnDate = DateTime.Today;
                    CS.UpdatedOn = DateTime.Today;
                    CS.ContactNo = customer.ContactNo;
                    CS.CustName = customer.CustName;
                    CS.CustAddress = customer.CustAddress;
                    CS.CustCode = customer.CustCode;
                    CS.customerprice = customer.customerprice;
                    CS.dealerprice = customer.dealerprice;
                    CS.dealerid = customer.dealerid;
                    CS.IsValid = customer.IsValid;
                    CS.password = customer.password;
                    CS.Email = customer.Email;
                   
                    db.Customers.Add(CS);
                    db.SaveChanges();
                return RedirectToAction("Index");


            }

            return RedirectToAction("Index");
           
        }



        DateTime ConvertToDate(string date)
        {
            DateTime cdate;
            date = date.Trim();
            if (date.Length != 10)
                throw new Exception("Invalid Date");
                //return Convert.ToDateTime("01/Jan/1900");
            string day=date.Substring(0,2), month=date.Substring(3,2), year=date.Substring(6,4);
            cdate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            return cdate;

        }
         //GET: Customers/Edit/5
        [AutorizeUser]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            Models.Customer CS = new Models.Customer();
            CS.ValidFrom =customer.ValidFrom.ToString("dd/MM/yyyy");
            CS.ValidTill =customer.ValidTill.ToString("dd/MM/yyyy");
            CS.EnteredOnDate = DateTime.Today.ToString("dd/MM/yyyy");
            //CS.UpdatedOn = DateTime.Today.ToString("dd/MM/yyyy");
            CS.ContactNo = customer.ContactNo;
            CS.CustName = customer.CustName;
            CS.CustAddress = customer.CustAddress;
            CS.CustCode = customer.CustCode;
            CS.customerprice = customer.customerprice;
            CS.dealerprice = customer.dealerprice;
            CS.dealerid = customer.dealerid;
            CS.IsValid = customer.IsValid;
            CS.password = customer.password;
            CS.Email = customer.Email;
            CS.CustomerId = customer.CustomerId;
            if (customer == null)
            {
                return HttpNotFound();
            }
            var q = db.Dealers.Where(x=>x.id==customer.dealerid).ToList();
            ViewBag.dealerid = new SelectList(q, "id", "name");
           
            return View(CS);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AutorizeUser]
        public ActionResult Edit(Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                Customer CS = db.Customers.Where(x=>x.CustomerId==customer.CustomerId).FirstOrDefault();

                //throw new Exception(customer.ValidFrom+","+customer.ValidTill);
                CS.ValidFrom = ConvertToDate(customer.ValidFrom);
                CS.ValidTill = ConvertToDate(customer.ValidTill);
                //CS.EnteredOnDate = DateTime.Today;
                CS.UpdatedOn = DateTime.Today;
                CS.ContactNo = customer.ContactNo;
                CS.CustName = customer.CustName;
                CS.CustAddress = customer.CustAddress;
                CS.CustCode = customer.CustCode;
                CS.customerprice = customer.customerprice;
                CS.dealerprice = customer.dealerprice;
                CS.dealerid = customer.dealerid;
                CS.IsValid = customer.IsValid;
                CS.password = customer.password;
                CS.Email = customer.Email;

                db.Entry(CS).State = EntityState.Modified;
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
