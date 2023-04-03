using WebApplication1.Filters;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Controllers
{
    [AutorizeUser]
    public class CustomersController : Controller
    {
        private IdCardDataManagerEntities1 db = new IdCardDataManagerEntities1();

        // GET: Customers
        [AutorizeUser]
        public ActionResult Index()
        {

            
            
            return View();
        }

        // GET: Customers/Details/5
        [AutorizeUser]
        public ActionResult Details(int? id)
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
            return View(customer);
        }

        // GET: Customers/Create
        [AutorizeUser]
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult Create([Bind(Include = "CustomerId,CustName,IsValid,ValidTill,EnteredOnDate,ValidFrom,CustAddress,ContactNo,Email,CustCode,UpdatedOn,UpdatedBy")] Customer customer)
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
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult Edit([Bind(Include = "CustomerId,CustName,IsValid,ValidTill,EnteredOnDate,ValidFrom,CustAddress,ContactNo,Email,CustCode,UpdatedOn,UpdatedBy")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
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
        public ActionResult SelectField(int id)
        {
            List<softwareoption> op = db.softwareoptions.Where(x => x.StSchool_SchoolId == id).ToList();
            if (op.Count > 0)
            {
                op[0].StSchool_SchoolId = id;
                DataTable dt = LinqToTable.LINQResultToDataTable(op);
                //ViewData.Model = sopt;
                ViewBag.optlist = dt;
            }
            else
            {
                ViewBag.optlist = null;

            }
            return View();
        }
        [AutorizeUser]
        [HttpPost]
        public ActionResult SelectField(softwareoption op)
        {
            db.Entry(op).State = EntityState.Modified;
            db.SaveChanges();
            List<softwareoption> op1 = db.softwareoptions.Where(x => x.StSchool_SchoolId == op.StSchool_SchoolId).ToList();
            op1[0].StSchool_SchoolId = op.StSchool_SchoolId;
            DataTable dt = LinqToTable.LINQResultToDataTable(op1);
            //ViewData.Model = sopt;
            ViewBag.optlist = dt;
            return View();
        }

        [AutorizeUser]
        public ActionResult School_Index()
        {
            var id = (int)Session["customerid"];
            var name = Session["custname"].ToString();
            var q = db.StoreAPKs.Where(x => x.name.ToLower() == name.ToLower()).FirstOrDefault();
            if (q != null)
            {
                ViewBag.apkname =q.name+"/"+ q.name + ".apk";

            }
            else
            {
                ViewBag.apkname = "citron/citron.apk";
            }

            var schools = db.Schools.Where(s => s.CustomerId == id);
            return View(schools.ToList());
        }

        [AutorizeUser]
        public ActionResult school_changepassword(int? id)
        {
            Models.Class1.ChangePassword ch = new Models.Class1.ChangePassword();
            ch.id = (int)id;
            return View(ch);
        }

        [HttpPost]
        [AutorizeUser]
        public ActionResult school_changepassword(Models.Class1.ChangePassword ch)
        {
            if(ch.id>0)
            {
                if(ch.ConfirmPassword==ch.NewPassword)
                {
                    var q = db.Schools.Where(x => x.SchoolId == ch.id && x.password == ch.OldPassword).FirstOrDefault();
                    if(q!=null)
                    {
                        q.password = ch.NewPassword;
                        db.Entry(q).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["smsg"] = "Change Success";

                    }
                    else
                    {
                        TempData["errormsg"] = "Old Password is Wrong..!";
                        return View(ch);
                    }

                }
                else
                {
                    TempData["errormsg"] = "Confirm Password and New Password Not Correct..!";
                    return View(ch);
                }

            }
            else
            {
                TempData["errormsg"] = "Select School..!";
                return View(ch);
            }
            return View(ch);
        }




        [AutorizeUser]
        public ActionResult customer_changepassword(int? id)
        {
            Models.Class1.ChangePassword ch = new Models.Class1.ChangePassword();
            ch.id = (int)Session["customerid"];
            return View(ch);
        }

        [HttpPost]
        [AutorizeUser]
        public ActionResult customer_changepassword(Models.Class1.ChangePassword ch)
        {
                if (ch.ConfirmPassword == ch.NewPassword)
                {
                    var q = db.Customers.Where(x => x.CustomerId == ch.id && x.password == ch.OldPassword).FirstOrDefault();
                    if (q != null)
                    {
                        q.password = ch.NewPassword;
                        db.Entry(q).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["smsg"] = "Change Success";

                    }
                    else
                    {
                        TempData["errormsg"] = "Old Password is Wrong..!";
                        return View(ch);
                    }

                }
                else
                {
                    TempData["errormsg"] = "Confirm Password and New Password Not Correct..!";
                    return View(ch);
                }
            return View(ch);
        }


        [AutorizeUser]
        public ActionResult School_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // GET: Schools/Create
        [AutorizeUser]
        public ActionResult School_Create()
        {
            int customerid = (int)Session["customerid"];
            var q = db.Customers.Where(x => x.CustomerId == customerid).ToList();
            ViewBag.CustomerId = new SelectList(q, "CustomerId", "CustName");
            return View();
        }

        // POST: Schools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AutorizeUser]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult School_Create([Bind(Include = "SchoolId,SchoolName,Address1,Address2,Address3,ContactNo,Email,Website,Logo,SchoolCode,IsActive,UpdatedOn,UpdatedBy,CustomerId,LogoUpload,Password")] School school)
        {
            if (ModelState.IsValid)
            {

                if(school.SchoolCode=="" || school.SchoolCode==null )
                {
                    TempData["errormsg"] = "School Code Required ..!";
                    ViewBag.CustomerId = new SelectList(db.Customers.Where(x=>x.CustomerId== school.CustomerId).ToList(), "CustomerId", "CustName");
                    return View(school);
                }

                else if(school.ContactNo=="" ||school.ContactNo==null)
                {
                    TempData["errormsg"] = "Contact Required ..!";
                    ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == school.CustomerId).ToList(), "CustomerId", "CustName");
                    return View(school);
                }

                var q = db.Schools.Where(x => x.SchoolCode.ToLower() == school.SchoolCode.ToLower()).ToList();
                if (q.Count>0)
                {
                    TempData["errormsg"] = "School Code Already Exist..!";
                    ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == school.CustomerId).ToList(), "CustomerId", "CustName");
                    return View(school);
                }


                school.UpdatedOn = DateTime.Today;
                school.updatedby = school.CustomerId;

                if (school.LogoUpload != null)
                {
                    var msg = "";

                    try
                    {
                        var sizeinbyte = school.LogoUpload.ContentLength;
                        var extension = Path.GetExtension(school.LogoUpload.FileName);
                        var sizeinkb = (sizeinbyte) / (1024);
                        if (sizeinkb > 200)
                        {
                            TempData["errormsg"] = "Photo Size should not be greator than 200 kb..!";
                            return RedirectToAction("School_Create");
                        }
                        else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                        {

                            var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), school.LogoUpload.FileName);
                            school.LogoUpload.SaveAs(ServerSavePath);
                            school.Logo = school.LogoUpload.FileName;
                        }
                        else
                        {
                            TempData["errormsg"] = "You must select an image file only.";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["errormsg"] = e.Message;
                        return RedirectToAction("School_Create");
                    }

                }

                db.Schools.Add(school);
                db.SaveChanges();

                try
                {
                    softwareoption op = new softwareoption();
                    op.Address = true;
                    op.admno = true;
                    op.ClassName = true;
                    op.Dob = true;
                    op.FatherName = true;
                    op.Mobile = true;
                    op.MotherName = true;
                    op.OptedConveyance = true;
                    op.Photo = true;
                    op.SectonName = true;
                    op.StName = true;
                    op.Session = true;
                    op.Res1 = true;
                    op.Res2 = true;
                   
                    op.StSchool_SchoolId = school.SchoolId;
                    db.softwareoptions.Add(op);
                    db.SaveChanges();
                    TempData["smsg"] = "Success";
                }
                catch (Exception ex)
                {
                    TempData["errormsg"]=ex.Message;
                    return RedirectToAction("Index");
                }

            }

            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == school.CustomerId).ToList(), "CustomerId", "CustName");
            return View(school);
        }




        // GET: Schools/Edit/5
        [AutorizeUser]
        public ActionResult Edit_School(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<School> school = db.Schools.Where(x => x.SchoolId == id).ToList();
            School st = school[0];

            if (school == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == st.CustomerId).ToList(), "CustomerId", "CustName");
            return View(st);
        }

        // POST: Schools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult Edit_School([Bind(Include = "SchoolId,SchoolName,Address1,Address2,Address3,ContactNo,Email,Website,Logo,SchoolCode,IsActive,UpdatedOn,UpdatedBy,CustomerId,LogoUpload,Password")] School school)
        {
            if (ModelState.IsValid)
            {

                if (school.SchoolCode == "" || school.SchoolCode == null)
                {
                    TempData["errormsg"] = "School Code Required ..!";
                    ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == school.CustomerId).ToList(), "CustomerId", "CustName");
                    return View(school);
                }

                else if (school.ContactNo == "" || school.ContactNo == null)
                {
                    TempData["errormsg"] = "Contact Required ..!";
                    ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == school.CustomerId).ToList(), "CustomerId", "CustName");
                    return View(school);
                }

                //var q = db.Schools.Where(x => x.SchoolCode.ToLower() == school.SchoolCode.ToLower()).ToList();
                //if (q.Count > 0)
                //{
                //    TempData["errormsg"] = "School Code Already Exist..!";
                //    ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustName", school.CustomerId);
                //    return View(school);
                //}







                school.updatedby = school.CustomerId;

                school.UpdatedOn = DateTime.Today;

                if (school.LogoUpload != null)
                {
                    var msg = "";

                    try
                    {
                        var sizeinbyte = school.LogoUpload.ContentLength;
                        var extension = Path.GetExtension(school.LogoUpload.FileName);
                        var sizeinkb = (sizeinbyte) / (1024);
                        if (sizeinkb > 200)
                        {
                            TempData["errormsg"] = "Photo Size should not be greator than 200 kb..!";
                            return RedirectToAction("School_Create");
                        }
                        else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                        {

                            var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), school.LogoUpload.FileName);
                            school.LogoUpload.SaveAs(ServerSavePath);
                            school.Logo = school.LogoUpload.FileName;
                        }
                        else
                        {
                            TempData["errormsg"] = "You must select an image file only.";
                            return RedirectToAction("School_Create");
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["errormsg"] = e.Message;
                        return RedirectToAction("School_Create");
                    }

                }

                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();
                TempData["smsg"] = "success";
                return RedirectToAction("School_Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == school.CustomerId).ToList(), "CustomerId", "CustName");
            return View(school);
        }

        [AutorizeUser]
        // GET: Schools/Delete/5
        public ActionResult Delete_School(int? id)
        {
            School school = db.Schools.Find(id);
            db.Schools.Remove(school);
            db.SaveChanges();
            TempData["smsg"] = "success";
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == school.CustomerId).ToList(), "CustomerId", "CustName");
            return RedirectToAction("School_Index");
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete_School")]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult DeleteConfirmed_School(int id)
        {
            School school = db.Schools.Find(id);
            db.Schools.Remove(school);
            db.SaveChanges();
            TempData["smsg"] = "success";
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.CustomerId == school.CustomerId).ToList(), "CustomerId", "CustName");
            return RedirectToAction("School_Index");
        }




        [AutorizeUser]
        public ActionResult Student_Index()
        {
            int customerid = (int)Session["customerid"];
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x=>x.CustomerId==customerid).ToList(), "SchoolId", "SchoolName");
             
            Student st = new Student();
            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault() ;
          st.sessionlist =db.session_master.ToList();

            st.Batch = 1;
            ViewBag.optlist1 = null;
            return View(st);

        }

        // GET: Students/Details/5
        [AutorizeUser]
        public ActionResult Student_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        [AutorizeUser]
        public ActionResult Student_Create()
        {


            int customerid = (int)Session["customerid"];
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x => x.CustomerId == customerid).ToList(), "SchoolId", "SchoolName");

            Student st = new Student();
            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();

            st.Batch = 1;
            
            return View(st);
        }

        [AutorizeUser]
        public ActionResult selectstudent(Student st)
        {
            var q = db.Schools.Where(x => x.SchoolId == st.StSchool_SchoolId).ToList();
            ViewBag.StSchool_SchoolId = new SelectList(q, "SchoolId", "SchoolName");

            List<softwareoption> op = db.softwareoptions.Where(s => s.StSchool_SchoolId == st.StSchool_SchoolId).ToList();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            //ViewData.Model = sopt;
            ViewBag.optlist = dt;

            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();

            return View("Student_Create", st);
        }


        [AutorizeUser]
        public ActionResult filterstudent(Student st)
        {
            int customerid = (int)Session["customerid"];
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x => x.CustomerId == customerid).ToList(), "SchoolId", "SchoolName");
            

            List<Student> op = db.Students.Where(s => s.StSchool_SchoolId == st.StSchool_SchoolId && s.Batch == st.Batch && s.Session == st.Session).ToList();

            if(st.ClassName!=null)
            {
                op = op.Where(x => x.ClassName == st.ClassName).ToList();
            }
            if (st.SectonName != null)
            {
                op = op.Where(x => x.SectonName == st.SectonName).ToList();
            }
            if(st.ReviewedBySchool!=false)
            {
                op = op.Where(x => x.ReviewedBySchool == st.ReviewedBySchool).ToList();
            }
            if (st.NeedUpdation != false)
            {
                op = op.Where(x => x.NeedUpdation == st.NeedUpdation).ToList();
            }
            if (st.AllCorrect != false)
            {
                op = op.Where(x => x.AllCorrect == st.AllCorrect).ToList();
            }

            DataTable dt = LinqToTable.LINQResultToDataTable(op);
                //ViewData.Model = sopt;
                ViewBag.optlist = dt;


            List<softwareoption> op1 = db.softwareoptions.Where(s => s.StSchool_SchoolId == st.StSchool_SchoolId).ToList();
            DataTable dt1 = LinqToTable.LINQResultToDataTable(op1);
            //ViewData.Model = sopt;
            ViewBag.optlist1 = dt1;
           
            st.sessionlist = db.session_master.ToList();
            return View("Student_Index",st);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult Student_Create([Bind(Include = "StudentId,AdmNo,StName,FatherName,MotherName,Dob,Mobile,Address,OptedConveyance,ClassName,SectonName,Session,Photo,Res1,Res2,ReviewedBySchool,ReviewDate,NeedUpdation,AllCorrect,UpdatedOn,StSchool_SchoolId,PhotoUpload,Batch,IsPhotoUpload,Excel_Photo,conveyance,stop,driver,designation,driver_mobile,rollno,house,blood_group,emergency_no")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.UpdatedOn = DateTime.Today;
                if (student.PhotoUpload != null)
                {
                    var msg = "";

                    try
                    {
                        var sizeinbyte = student.PhotoUpload.ContentLength;
                        var extension = ".jpg";
                        var sizeinkb = (sizeinbyte) / (1024);
                        if (sizeinkb > 200)
                        {
                            TempData["errormsg"] = "Photo Size should not be greator than 200 kb..!";
                            return RedirectToAction("Index");
                        }
                        else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                        {
                            var schoolcode = db.Schools.Where(x => x.SchoolId == student.StSchool_SchoolId).FirstOrDefault();

                            var filename = "";
                            if (student.StudentId > 0)
                            {

                                filename = student.StudentId + student.Session + schoolcode.SchoolCode + extension;
                            }
                            else
                            {
                                int maxstid = db.Students.ToList().Select(x => x.StudentId).Max();
                                filename = maxstid + 1 + student.Session + schoolcode.SchoolCode + extension;
                            }

                            var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), filename);
                            System.IO.File.Delete(ServerSavePath);

                            student.PhotoUpload.SaveAs(ServerSavePath);
                            student.Photo = filename;
                            student.IsPhotoUpload = true;
                        }
                        else
                        {
                            TempData["errormsg"] = "You must select an image file only.";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["errormsg"] = e.Message;
                        return RedirectToAction("Index");
                    }

                }
                student.UpdatedOn = DateTime.Now;
                db.Students.Add(student);
                db.SaveChanges();
                TempData["smsg"] = "success";
                return RedirectToAction("filterstudent", student);
            }

            ViewBag.StSchool_SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", student.StSchool_SchoolId);
            return View(student);
        }

        // GET: Students/Edit/5
        [AutorizeUser]
        public ActionResult Student_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            List<softwareoption> op = db.softwareoptions.Where(s => s.StSchool_SchoolId == student.StSchool_SchoolId).ToList();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            //ViewData.Model = sopt;
            ViewBag.optlist = dt;

            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x=>x.SchoolId== student.StSchool_SchoolId), "SchoolId", "SchoolName", student.StSchool_SchoolId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult Student_Edit([Bind(Include = "StudentId,AdmNo,StName,FatherName,MotherName,Dob,Mobile,Address,OptedConveyance,ClassName,SectonName,Session,Photo,Res1,Res2,ReviewedBySchool,ReviewDate,NeedUpdation,AllCorrect,UpdatedOn,StSchool_SchoolId,PhotoUpload,Batch,IsPhotoUpload,Excel_Photo,conveyance,stop,driver,designation,driver_mobile,rollno,house,blood_group,emergency_no")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.UpdatedOn = DateTime.Now;
                if (student.PhotoUpload != null)
                {
                    var msg = "";

                    try
                    {
                        var sizeinbyte = student.PhotoUpload.ContentLength;
                        var extension = ".jpg";
                        var sizeinkb = (sizeinbyte) / (1024);
                        if (sizeinkb > 200)
                        {
                            TempData["errormsg"] = "Photo Size should not be greator than 200 kb..!";
                            return RedirectToAction("Index");
                        }
                        else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                        {
                            var schoolcode = db.Schools.Where(x => x.SchoolId == student.StSchool_SchoolId).FirstOrDefault();

                            var filename = "";
                            if (student.StudentId > 0)
                            {

                                filename = student.StudentId + student.Session + schoolcode.SchoolCode + extension;
                            }
                            else
                            {
                                int maxstid = db.Students.ToList().Select(x => x.StudentId).Max();
                                filename = maxstid + 1 + student.Session + schoolcode.SchoolCode + extension;
                            }

                            var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"),filename);
                            System.IO.File.Delete(ServerSavePath);
                            student.PhotoUpload.SaveAs(ServerSavePath);
                            student.Photo = filename;
                            student.IsPhotoUpload = true;
                        }
                        else
                        {
                            TempData["errormsg"] = "You must select an image file only.";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["errormsg"] = e.Message;
                        return RedirectToAction("Index");
                    }

                }
                //student.UpdatedOn = DateTime.Now;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                TempData["smsg"] = "success";
                Student st = new Student();
                st.StSchool_SchoolId = student.StSchool_SchoolId;
                st.Session = student.Session;
                st.Batch = student.Batch;
                return RedirectToAction("filterstudent", st);
               
            }
            List<softwareoption> op = db.softwareoptions.Where(s => s.StSchool_SchoolId == student.StSchool_SchoolId).ToList();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            //ViewData.Model = sopt;
            ViewBag.optlist = dt;
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", student.StSchool_SchoolId);
            return View(student);

        }

        // GET: Students/Delete/5
        [AutorizeUser]
        public ActionResult Student_Delete(int? id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            TempData["smsg"] = "success";

            Student st = new Student();
            st.StSchool_SchoolId = student.StSchool_SchoolId;
            st.Session = student.Session;
            st.Batch = student.Batch;
            return RedirectToAction("filterstudent",st);
        }

        // POST: Students/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[AutorizeUser]
        //public ActionResult Student_DeleteConfirmed(int id)
        //{
        //    Student student = db.Students.Find(id);
        //    db.Students.Remove(student);
        //    db.SaveChanges();
        //    TempData["smsg"] = "success";
        //    return RedirectToAction("Student_Index");
        //}


        [AutorizeUser]
        public ActionResult DeleteAllStudent(Student st)
        {
           List<Student> q = db.Students.Where(x=>x.Session==st.Session && x.Batch==st.Batch && x.StSchool_SchoolId==st.StSchool_SchoolId).ToList();

            db.Students.RemoveRange(q);
            db.SaveChanges();
            TempData["smsg"] = "success";
            return RedirectToAction("Student_Index");
        }





        [AutorizeUser]
        public ActionResult ImportExcel()
        {
            Student st = new Student();
            st.Batch = 1;
            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();
            int customerid = (int)Session["customerid"];
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x => x.CustomerId == customerid).ToList(), "SchoolId", "SchoolName");

            return View(st);
        }

        [HttpPost]
        [AutorizeUser]
        public ActionResult ImportExcel(WebApplication1.Student st)
        {
            DateTime UpdatedOn = DateTime.Now;
            
            var photo = st.PhotoUpload.FileName; 
            var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), photo);
            st.PhotoUpload.SaveAs(ServerSavePath);
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ServerSavePath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                DataTable excelDataSet = new DataTable();
                conn.Open();
                OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter
                ("select * from [sheet1$]", conn);

                objDA.Fill(excelDataSet);
                int ctr = 1;
                foreach (DataRow dr in excelDataSet.Rows)
                {
                    Student stud = new Student();
                    stud.Address = (Convert.IsDBNull(dr["address"]) ? "" : dr["address"].ToString());
                    stud.AdmNo = (Convert.IsDBNull(dr["admno"])?"": dr["admno"].ToString());
                    stud.rollno = Convert.IsDBNull(dr["rollno"]).ToString();

                    stud.AllCorrect = false;
                    stud.ClassName = (Convert.IsDBNull(dr["Class"])?"": dr["Class"].ToString());
                    stud.Dob = (Convert.IsDBNull(dr["dob"])?"": dr["dob"].ToString());
                    stud.FatherName = (Convert.IsDBNull(dr["Father"])?"": dr["Father"].ToString());
                    stud.MotherName = (Convert.IsDBNull(dr["Mother"])?"": dr["Mother"].ToString());
                    stud.Mobile = (Convert.IsDBNull(dr["Mobile"])?"": dr["Mobile"].ToString());
                    stud.NeedUpdation = true;
                   stud.OptedConveyance = false;
                  stud.Res1 = (Convert.IsDBNull(dr["res1"])?"": dr["res1"].ToString());
                   stud.Res2 = (Convert.IsDBNull(dr["res2"])?"": dr["res2"].ToString());
                    stud.ReviewDate = DateTime.Today;
                    stud.UpdatedOn = DateTime.Today;
                    stud.StName = (Convert.IsDBNull(dr["StName"])?"": dr["StName"].ToString());
                    stud.Session = st.Session;
                    stud.SectonName = (Convert.IsDBNull(dr["Section"])?"": dr["Section"].ToString());
                    stud.StSchool_SchoolId =st.StSchool_SchoolId;
                    stud.ReviewedBySchool = false;
                    stud.IsPhotoUpload = false;
                    stud.Photo = "";
                    stud.Excel_Photo= (Convert.IsDBNull(dr["photo"])?"": dr["photo"].ToString());
                    stud.Batch = st.Batch;
                    stud.house=( Convert.IsDBNull(dr["house"])?"": dr["house"].ToString());
                    stud.driver = (Convert.IsDBNull(dr["driver_name"])?"": dr["driver_name"].ToString());
                    stud.driver_mobile =( Convert.IsDBNull(dr["driver_mobile"])?"": dr["driver_mobile"].ToString());
                    stud.conveyance = (Convert.IsDBNull(dr["conveyance"])?"": dr["conveyance"].ToString());
                    stud.stop = (Convert.IsDBNull(dr["stop"])?"": dr["stop"].ToString());
                    stud.designation = (Convert.IsDBNull(dr["designation"])?"": dr["designation"].ToString());
                    if(dr["emergency_no"].ToString()==null|| dr["emergency_no"].ToString() == "")
                    {
                        stud.emergency_no = 0;
                    }
                    else
                    {
                        stud.emergency_no = Convert.ToInt64(dr["emergency_no"]);
                    }
                    
                    stud.bloodgroup = (Convert.IsDBNull(dr["bloodgroup"])?"": dr["bloodgroup"].ToString());
                    db.Students.Add(stud);
                    db.SaveChanges();
                    TempData["smsg"] = "success";
                }


            }
            st.Batch = 1;
            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();
            int customerid = (int)Session["customerid"];
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x => x.CustomerId == customerid).ToList(), "SchoolId", "SchoolName");

            return View(st);

        }
        [HttpPost]
        [AutorizeUser]
        public FileResult ExportToExcel(WebApplication1.Student st)
        {
           
            List<Student> op = db.Students.Where(s => s.StSchool_SchoolId == st.StSchool_SchoolId && s.Batch == st.Batch && s.Session == st.Session).ToList();

            if (st.ClassName != null)
            {
                op = op.Where(x => x.ClassName == st.ClassName).ToList();
            }
            if (st.SectonName != null)
            {
                op = op.Where(x => x.SectonName == st.SectonName).ToList();
            }
            if (st.ReviewedBySchool != false)
            {
                op = op.Where(x => x.ReviewedBySchool == st.ReviewedBySchool).ToList();
            }
            if (st.NeedUpdation != false)
            {
                op = op.Where(x => x.NeedUpdation == st.NeedUpdation).ToList();
            }
            if (st.AllCorrect != false)
            {
                op = op.Where(x => x.AllCorrect == st.AllCorrect).ToList();
            }

            List<Student> op1 = new List<Student>();
            List<Student> photo_folder = new List<Student>();
            foreach (var item in op)
            {
                Student st1 = new Student();
                
                if (item.Photo!=null)
                {
                  var  a = item.Photo.Split('.');
                    st1.Photo = a[0];
                }
                else
                {
                    st1.Photo = item.Photo;
                }
                st1.AdmNo = item.AdmNo;
                st1.Address = item.Address;
                st1.Batch = item.Batch;
                st1.ClassName = item.ClassName;
                st1.SectonName = item.SectonName;
                st1.Session = item.Session;
              
                st1.Dob =item.Dob;
                st1.FatherName = item.FatherName;
                st1.Mobile = item.Mobile;
                st1.MotherName = item.MotherName;
                st1.StName = item.StName;
                st1.AllCorrect = item.AllCorrect;
                st1.NeedUpdation = item.NeedUpdation;
                st1.ReviewedBySchool = item.ReviewedBySchool;
                st1.OptedConveyance = item.OptedConveyance;
                st1.Res1 = item.Res1;
                st1.Res2 = item.Res2;
                st1.rollno = item.rollno;
                st1.conveyance = item.conveyance;
                st1.driver = item.driver;
                st1.driver_mobile = item.driver_mobile;
                st1.designation = item.designation;
                st1.stop = item.stop;
                st1.emergency_no = item.emergency_no;
                st1.bloodgroup = item.bloodgroup;
                op1.Add(st1);
  

                }



            
            string dirUrl ="/ExcelWorks/photos"+st.StSchool_SchoolId;

            
            string dirPath = Server.MapPath(dirUrl);
           


            string zipname = "/ExcelWorks/" + st.StSchool_SchoolId+".zip";



            dirPath = Server.MapPath(dirUrl);

            // Check for Directory, If not exist, then create it  



            // save the file to the Specifyed folder 
            string source = Server.MapPath("/uploads");
            FileInfo fi = new FileInfo(source);

            if (Directory.Exists(dirPath) == false)
            {
                Directory.CreateDirectory(dirPath);
            }
            else
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(dirPath);

               // di.Delete(true);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                di.Delete(true);
               
                //foreach (DirectoryInfo dir in di.GetDirectories())
                //{
                //    dir.Delete(true);
                //}
                var z = Server.MapPath(zipname);
                System.IO.FileInfo di1 = new FileInfo(z);
                di1.Delete();


                Directory.CreateDirectory(dirPath);
            }

            foreach (var item in op)
            {
                if (item.Photo != null)
                {
                    var q = item.Photo.Split('?', ' ');


                    //System.IO.File.Delete(dirPath + "\\" + q[0]);
                    var a = source + "\\" + q[0];
                    if (System.IO.File.Exists(a))
                    {
                        var b = dirPath + "\\" + q[0];
                        if (!System.IO.File.Exists(b))
                        {
                            System.IO.File.Copy(source + "\\" + q[0], dirPath + "\\" + q[0]);
                        }
                    }
                }
            }



            var storefolder = "/ExcelWorks/photos" + st.StSchool_SchoolId;

            string fileName = "studentlist_ "+ st.StSchool_SchoolId+"_ "+".xlsx";
            DataTable dt = LinqToTable.LINQResultToDataTable(op1);
            using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.Worksheets.Add(dt, "studentlist");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename=studentlist.xlsx");
                using (System.IO.MemoryStream MyMemoryStream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(Server.MapPath(storefolder+"/"+fileName));
                    //MyMemoryStream.WriteTo(Response.OutputStream);
                    //Response.Flush();
                    //Response.End();
                }
            }

            
                string pathname = Server.MapPath(storefolder);
                string[] filename = Directory.GetFiles(pathname);
                using (ZipFile zip = new ZipFile())
                {
                
                    zip.AddFiles(filename, "file");
                    zip.Save(Server.MapPath(zipname));
                    //lbltxt.Text = "Zip File Created";
                }

            //return File(Server.MapPath(zipname),
            //     "application/zip", st.StSchool_SchoolId + ".zip");

            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(zipname));
            return new FileContentResult(fileBytes, "application/zip");

            //return File(fileBytes, "application/zip", "photos" + st.StSchool_SchoolId + ".zip");
        }


        [AutorizeUser]
        public ActionResult ViewParentLogin()
        {
            int customerid = (int)Session["customerid"];
           
                var op = (from m in db.ParentLogins.ToList()
                          join a in db.Schools.Where(x => x.CustomerId == customerid) on m.schoolid equals a.SchoolId
                          select new ParentLogin
                          {
                              id=m.id,
                              isactive=m.isactive,
                              schoolid=m.schoolid,
                              schoolname=a.SchoolName,
                             username= m.username,
                             upassword=m.upassword,
                          }
                         );
           
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            ViewBag.parentloginlist = dt;
            return View();
        }

        [AutorizeUser]
        public ActionResult AddParentLogin()
        {
            int customerid = (int)Session["customerid"];
            ViewBag.schoolid = new SelectList(db.Schools.Where(x => x.CustomerId == customerid).ToList(), "SchoolId", "SchoolName");

            
            return View();
        }

        [HttpPost]
        [AutorizeUser]
        public ActionResult AddParentLogin(WebApplication1.ParentLogin pl)
        {
            if(ModelState.IsValid)
            {
                ParentLogin p = new ParentLogin();
                p.schoolid = pl.schoolid;
                p.username = pl.username;
                p.upassword = pl.upassword;
                p.isactive = true;
                db.ParentLogins.Add(p);
                db.SaveChanges();
                TempData["smsg"] = "success";
            }
            int customerid = (int)Session["customerid"];
            ViewBag.schoolid = new SelectList(db.Schools.Where(x => x.CustomerId == customerid).ToList(), "SchoolId", "SchoolName");

            return View();
        }


        [HttpPost]
        [AutorizeUser]
        public ActionResult viewidcard(WebApplication1.Student st)
        {

            List<Student> op = db.Students.Where(s => s.StSchool_SchoolId == st.StSchool_SchoolId && s.Batch == st.Batch && s.Session == st.Session).ToList();

            if (st.ClassName != null)
            {
                op = op.Where(x => x.ClassName == st.ClassName).ToList();
            }
            if (st.SectonName != null)
            {
                op = op.Where(x => x.SectonName == st.SectonName).ToList();
            }
            if (st.ReviewedBySchool != false)
            {
                op = op.Where(x => x.ReviewedBySchool == st.ReviewedBySchool).ToList();
            }
            if (st.NeedUpdation != false)
            {
                op = op.Where(x => x.NeedUpdation == st.NeedUpdation).ToList();
            }
            if (st.AllCorrect != false)
            {
                op = op.Where(x => x.AllCorrect == st.AllCorrect).ToList();
            }

            List<Student> op1 = new List<Student>();
         
            foreach (var item in op)
            {
                Student st1 = new Student();
                st1.AdmNo = item.AdmNo;
                st1.Address = item.Address;
                st1.Batch = item.Batch;
                st1.ClassName = item.ClassName;
                st1.SectonName = item.SectonName;
                st1.Session = item.Session;
                st1.Photo = item.Photo;
                st1.Dob =item.Dob;
                st1.FatherName = item.FatherName;
                st1.Mobile = item.Mobile;
                st1.MotherName = item.MotherName;
                st1.StName = item.StName;
                st1.AllCorrect = item.AllCorrect;
                st1.NeedUpdation = item.NeedUpdation;
                st1.ReviewedBySchool = item.ReviewedBySchool;
                st1.OptedConveyance = item.OptedConveyance;
                st1.Res1 = item.Res1;
                st1.Res2 = item.Res2;
                st1.School = db.Schools.Where(x=>x.SchoolId==item.StSchool_SchoolId).FirstOrDefault();
                st1.School.SchoolName = st1.School.SchoolName;
                st1.School.Logo = st1.School.Logo;
                st1.rollno = item.rollno;
                st1.conveyance = item.conveyance;
                st1.driver = item.driver;
                st1.driver_mobile = item.driver_mobile;
                st1.designation = item.designation;
                st1.stop = item.stop;
                st1.emergency_no = item.emergency_no;
                st1.bloodgroup = item.bloodgroup;
                op1.Add(st1);
            }

            ViewBag.studentlist = op1.ToList();

            return View();
        }


        [AutorizeUser]
        public ActionResult UploadPhoto()
        {
            int customerid = (int)Session["customerid"];
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x => x.CustomerId == customerid).ToList(), "SchoolId", "SchoolName");

          
            WebApplication1.Models.BulkImage st = new Models.BulkImage();
            st.session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();
            return View(st);
        }

        [HttpPost]
        [AutorizeUser]
        public ActionResult UploadFile(WebApplication1.Models.bulhimagetest student)
        {
            var dirPath = Server.MapPath("/zipfile");
            if (student.Files != null)
            {
                foreach (HttpPostedFileBase file in student.Files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension.ToUpper() != ".ZIP")
                    {
                        extension = "jpg";
                        var st = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.session && x.Excel_Photo == file.FileName).FirstOrDefault();

                        if (st != null)
                        {
                            var msg = "";

                            try
                            {
                                var sizeinbyte = file.ContentLength;

                                var sizeinkb = (sizeinbyte) / (1024);
                                if (sizeinkb > 500)
                                {
                                    TempData["errormsg"] = "Photo Size should not be greator than 500 kb..!";
                                    return RedirectToAction("Index");
                                }
                                else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                                {

                                    var filename = st.AdmNo + st.Session + st.School.SchoolCode + extension;

                                    var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), filename);
                                    System.IO.File.Delete(ServerSavePath);
                                    file.SaveAs(ServerSavePath);
                                    st.IsPhotoUpload = true;
                                    st.Photo = filename;
                                    db.Entry(st).State = EntityState.Modified;
                                    db.SaveChanges();

                                }
                                else
                                {
                                    TempData["errormsg"] = "You must select an image file only.";
                                    return Json(TempData["errormsg"], JsonRequestBehavior.AllowGet);
                                 
                                }
                            }
                            catch (Exception e)
                            {
                                TempData["errormsg"] = e.Message;
                                return Json(TempData["errormsg"], JsonRequestBehavior.AllowGet);
                                //return RedirectToAction("UploadPhoto");
                            }
                            return Json("Updated..!", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(" There Are No Record with this Filename..!", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(dirPath);
                        Directory.CreateDirectory(dirPath + "\\" + file.FileName.Replace(".zip", "").Trim());
                        using (ZipFile zip1 = ZipFile.Read(file.InputStream))
                        {
                            var selection = (from e in zip1.Entries
                                             select e);
                            foreach (var e in selection)
                            {

                                e.Extract(dirPath);
                            }
                        }
                        DirectoryInfo Folder;
                        FileInfo[] images;
                        Folder = new DirectoryInfo(dirPath + "\\" + file.FileName.Replace(".zip", "").Trim());
                        images = Folder.GetFiles();
                        for (int i = 0; i < images.Length; i++)
                        {
                            var img = images[i].Name;
                            var st = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.session && x.Excel_Photo == img).FirstOrDefault();

                            if (st != null)
                            {
                                var msg = "";

                                try
                                {
                                    var sizeinbyte = images[i].Length;

                                    var sizeinkb = (sizeinbyte) / (1024);
                                    if (sizeinkb > 500)
                                    {
                                        TempData["errormsg"] = "Photo Size should not be greator than 500 kb..!";
                                        return RedirectToAction("Index");
                                    }
                                    else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                                    {

                                        var filename = st.AdmNo + st.Session + st.School.SchoolCode + "jpg";

                                        var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), filename);
                                        System.IO.File.Delete(ServerSavePath);
                                        file.SaveAs(ServerSavePath);
                                        st.IsPhotoUpload = true;
                                        st.Photo = filename;
                                        db.Entry(st).State = EntityState.Modified;
                                        db.SaveChanges();


                                    }
                                    else
                                    {
                                        TempData["errormsg"] = "You must select an image file only.";
                                        return RedirectToAction("UploadPhoto");
                                    }
                                }
                                catch (Exception e)
                                {
                                    TempData["errormsg"] = e.Message;
                                    return RedirectToAction("UploadPhoto");
                                }

                            }
                        }

                        var query = Directory.GetFiles(dirPath + "\\" + file.FileName.Replace(".zip", "").Trim());
                        foreach (var item in query)
                        {
                            System.IO.File.Delete(item);
                        }
                        var f = dirPath + "\\" + file.FileName.Replace(".zip", "").Trim();

                        Directory.Delete(f);
                        Directory.Delete(dirPath);
                        return Json("Updated..!", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json("File NoT Select..!",JsonRequestBehavior.AllowGet);
        }

       [HttpPost]
        [AutorizeUser]
        public ActionResult UploadPhoto(WebApplication1.Models.BulkImage student)
        {
            var dirPath = Server.MapPath("/zipfile");
            if (student.Files != null)
            {
                foreach (HttpPostedFileBase file in student.Files)
                {
                    var extension = Path.GetExtension(file.FileName);
                   
                    if (extension.ToUpper()!=".ZIP")
                    {
                        var st = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.session && x.Excel_Photo == file.FileName).FirstOrDefault();

                        if (st != null)
                        {
                            var msg = "";

                            try
                            {
                                var sizeinbyte = file.ContentLength;

                                var sizeinkb = (sizeinbyte) / (1024);
                                if (sizeinkb > 500)
                                {
                                    TempData["errormsg"] = "Photo Size should not be greator than 500 kb..!";
                                    return RedirectToAction("Index");
                                }
                                else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                                {

                                    var filename = st.AdmNo + st.Session + st.School.SchoolCode + extension;

                                    var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), filename);
                                    System.IO.File.Delete(ServerSavePath);
                                    file.SaveAs(ServerSavePath);
                                    st.IsPhotoUpload = true;
                                    st.Photo = filename;
                                    db.Entry(st).State = EntityState.Modified;
                                    db.SaveChanges();

                                }
                                else
                                {
                                    TempData["errormsg"] = "You must select an image file only.";
                                    return RedirectToAction("UploadPhoto");
                                }
                            }
                            catch (Exception e)
                            {
                                TempData["errormsg"] = e.Message;
                                return RedirectToAction("UploadPhoto");
                            }
                        }
                    }
                    else
                    {
                      
                        Directory.CreateDirectory(dirPath);
                        using (ZipFile zip1 = ZipFile.Read("D:\\system_format\\pic.zip"))
                        {
                            var selection = (from e in zip1.Entries
                                             select e);
                            foreach (var e in selection)
                            {

                                e.Extract(dirPath);
                            }
                        }

                        DirectoryInfo Folder;
                        FileInfo[] images;
                        Folder =new DirectoryInfo(dirPath+"/"+file.FileName.Replace(".zip","").Trim());
                        images = Folder.GetFiles();
                        for (int i = 0; i < images.Length; i++)
                        {
                            var st = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.session && x.Excel_Photo == images[i].Name).FirstOrDefault();

                            if (st!=null)
                            {
                                var msg = "";

                                try
                                {
                                    var sizeinbyte = images[i].Length;

                                    var sizeinkb = (sizeinbyte) / (1024);
                                    if (sizeinkb > 500)
                                    {
                                        TempData["errormsg"] = "Photo Size should not be greator than 500 kb..!";
                                        return RedirectToAction("Index");
                                    }
                                    else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                                    {

                                        var filename = st.AdmNo + st.Session + st.School.SchoolCode + images[i].Extension;

                                        var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), filename);
                                        System.IO.File.Delete(ServerSavePath);
                                        file.SaveAs(ServerSavePath);
                                        st.IsPhotoUpload = true;
                                        st.Photo = filename;
                                        db.Entry(st).State = EntityState.Modified;
                                        db.SaveChanges();
                                        TempData["smsg"] = "success";

                                    }
                                    else
                                    {
                                        TempData["errormsg"] = "You must select an image file only.";
                                        return RedirectToAction("UploadPhoto");
                                    }
                                }
                                catch (Exception e)
                                {
                                    TempData["errormsg"] = e.Message;
                                    return RedirectToAction("UploadPhoto");
                                }

                            }
                        }

                        var query = Directory.GetFiles(dirPath + "/" + file.FileName.Replace(".zip", "").Trim());
                        foreach (var item in query)
                        {
                            Directory.Delete(item);
                        }
                        Directory.Delete(dirPath);



                    }
                }
            }

            int customerid = (int)Session["customerid"];
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x => x.CustomerId == customerid).ToList(), "SchoolId", "SchoolName");
            WebApplication1.Models.BulkImage st1 = new Models.BulkImage();
            st1.session = "2022-2023";
            return View(st1);
        }

        [HttpPost]
        [AutorizeUser]
        public ActionResult UpdateParentLogin(int? id)
        {
            var q = db.ParentLogins.Where(x => x.id == id).FirstOrDefault();
            if(q.isactive)
            {
                q.isactive = false;
            }
            else
            {
                q.isactive = true;

            }
           
            db.Entry(q).State = EntityState.Modified;
            db.SaveChanges();
            TempData["smsg"] = "success";
            var op = db.ParentLogins.ToList();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            ViewBag.parentloginlist = dt;
            return RedirectToAction("ViewParentLogin");
        }


       
    }
}
