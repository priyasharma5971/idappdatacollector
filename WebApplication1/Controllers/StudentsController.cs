using WebApplication1.Filters;
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
    public class StudentsController : Controller
    {
        private IdCardDataManagerEntities1 db = new IdCardDataManagerEntities1();

        // GET: Students
        [AutorizeUser]
        public ActionResult Index()
        {
            //var students = db.Students.Where(s => s.StSchool_SchoolId==id);
            var id = (int)Session["schoolid"];
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x=>x.SchoolId==id).ToList(), "SchoolId", "SchoolName");
           
            Student st = new Student();
            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();
            st.Batch = 1;
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == id && x.Session == st.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId ==id && x.Session == st.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName");

            var name = Session["custname"].ToString();
            var q = db.StoreAPKs.Where(x => x.name.ToLower() == name.ToLower()).FirstOrDefault();
            if (q != null)
            {
                ViewBag.apkname = q.name + "/" + q.name + ".apk";

            }
            else
            {
                ViewBag.apkname = "citron/citron.apk";
            }





            return View(st);
        }

        // GET: Students/Details/5
        [AutorizeUser]
        public ActionResult Details(int? id)
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
        public ActionResult Create()
        {
            int schoolid = (int)Session["schoolid"];
            Student st = new Student();
            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();
            st.Batch = 1;
            var q = db.Schools.Where(x => x.SchoolId == schoolid).ToList();
            ViewBag.StSchool_SchoolId = new SelectList(q, "SchoolId", "SchoolName");
            
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == schoolid && x.Session == st.Session).GroupBy(x=>x.ClassName).Select(x=>x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName"); 
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == schoolid && x.Session == st.Session).GroupBy(x=>x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName"); 
            return View(st);
        }

        [AutorizeUser]
        public ActionResult selectstudent(Student st)
        {
            var q = db.Schools.Where(x => x.SchoolId == st.StSchool_SchoolId).ToList();
            ViewBag.StSchool_SchoolId = new SelectList(q, "SchoolId", "SchoolName");
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == st.StSchool_SchoolId && x.Session == st.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == st.StSchool_SchoolId && x.Session == st.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName");

            List<softwareoption> op = db.softwareoptions.Where(s => s.StSchool_SchoolId == st.StSchool_SchoolId).ToList();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            //ViewData.Model = sopt;
            ViewBag.optlist = dt;
            st.Batch = 1;

           
            st.sessionlist = db.session_master.ToList();
            return View("Create",st);
        }

        [AutorizeUser]
        public ActionResult filterstudent(Student st)
        {
            var q = db.Schools.Where(x => x.SchoolId == st.StSchool_SchoolId).ToList();
            ViewBag.StSchool_SchoolId = new SelectList(q, "SchoolId", "SchoolName");
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == st.StSchool_SchoolId && x.Session == st.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == st.StSchool_SchoolId && x.Session == st.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName");

            List<Student> op = db.Students.Where(s => s.StSchool_SchoolId == st.StSchool_SchoolId && s.Batch == st.Batch && s.Session == st.Session).ToList();
            if(st.ClassName!=null)
            {
                op = op.Where(x => x.ClassName == st.ClassName).ToList();
            }
            if(st.SectonName!=null)
            {
                op = op.Where(x => x.SectonName == st.SectonName).ToList();
            }
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            st.Batch = 1;
            ViewBag.optlist = dt;


            
            st.sessionlist = db.session_master.ToList();
            return View("Index",st);
        }
        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult Create([Bind(Include = "StudentId,AdmNo,StName,FatherName,MotherName,Dob,Mobile,Address,OptedConveyance,ClassName,SectonName,Session,Photo,Res1,Res2,ReviewedBySchool,ReviewDate,NeedUpdation,AllCorrect,UpdatedOn,StSchool_SchoolId,PhotoUpload,Batch,IsPhotoUpload,Excel_Photo,conveyance,stop,driver,designation,driver_mobile,rollno,house,blood_group,emergency_no")] Student student)
        {
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName");
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


                            var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), filename);
                            student.PhotoUpload.SaveAs(ServerSavePath);
                            student.Photo = filename;
                            student.IsPhotoUpload = true;
                            student.Size = sizeinkb;
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
                TempData["smsg"] = "success";
                db.Students.Add(student);
                db.SaveChanges();
                
            }

            ViewBag.StSchool_SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", student.StSchool_SchoolId);
            student.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            student.sessionlist = db.session_master.ToList();
            return View(student);
        }

        // GET: Students/Edit/5
        [AutorizeUser]
        public ActionResult Edit(int? id)
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
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools.Where(x => x.SchoolId == student.StSchool_SchoolId), "SchoolId", "SchoolName", student.StSchool_SchoolId);
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName");
            List<softwareoption> op = db.softwareoptions.Where(s => s.StSchool_SchoolId == student.StSchool_SchoolId).ToList();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            //ViewData.Model = sopt;
            ViewBag.optlist = dt;
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeUser]
        public ActionResult Edit([Bind(Include = "StudentId,AdmNo,StName,FatherName,MotherName,Dob,Mobile,Address,OptedConveyance,ClassName,SectonName,Session,Photo,Res1,Res2,ReviewedBySchool,ReviewDate,NeedUpdation,AllCorrect,UpdatedOn,StSchool_SchoolId,PhotoUpload,Batch,IsPhotoUpload,Excel_Photo,conveyance,stop,driver,designation,driver_mobile,rollno,house,blood_group,emergency_no")] Student student)
        {
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName");
            List<softwareoption> op = db.softwareoptions.Where(s => s.StSchool_SchoolId == student.StSchool_SchoolId).ToList();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            student.sessionlist = db.session_master.ToList();
            //ViewData.Model = sopt;
            ViewBag.optlist = dt;
            if (ModelState.IsValid)
            {
                student.UpdatedOn = DateTime.Now;
                if (student.PhotoUpload!=null)
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
                            student.Size = sizeinkb;
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
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                TempData["smsg"] = "success";
                Student st = new Student();
                st.StSchool_SchoolId = student.StSchool_SchoolId;
                st.Session = student.Session;
                st.Batch = student.Batch;
                return RedirectToAction("filterstudent", st);
                //return RedirectToAction("Index", new { id = student.StSchool_SchoolId });
            }
            ViewBag.StSchool_SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", student.StSchool_SchoolId);
          
            
            return View(student);
       
        }

        [HttpPost]
        public ActionResult AllCorrect(int? id)
        {
            Student student = db.Students.Find(id);
            student.NeedUpdation = false;
            student.AllCorrect = true;
            student.ReviewedBySchool = true;
            student.ReviewDate = DateTime.Today;
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            TempData["smsg"] = "success";

            var classlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName");


            var q = db.Schools.Where(x => x.SchoolId == student.StSchool_SchoolId).ToList();
            ViewBag.StSchool_SchoolId = new SelectList(q, "SchoolId", "SchoolName");
           

            List<Student> op = db.Students.Where(s => s.StSchool_SchoolId == student.StSchool_SchoolId && s.Batch == student.Batch && s.Session == student.Session).ToList();
            
            Student st = new Student();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            st.Batch = 1;
            st.Session = student.Session;
            st.StSchool_SchoolId = student.StSchool_SchoolId;
            ViewBag.optlist = dt;



            return View("Index", st);


            //return RedirectToAction("Index",new { id=student.StSchool_SchoolId});
        }

        [HttpPost]
        public ActionResult NeedUpdation(int? id)
        {
            Student student = db.Students.Find(id);
            student.AllCorrect = false;
            student.NeedUpdation = true;
            student.ReviewedBySchool = true;
            student.ReviewDate = DateTime.Today;
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            TempData["smsg"] = "success";
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectonName = new SelectList(sectionlist, "SectonName", "SectonName");


            var q = db.Schools.Where(x => x.SchoolId == student.StSchool_SchoolId).ToList();
            ViewBag.StSchool_SchoolId = new SelectList(q, "SchoolId", "SchoolName");


            List<Student> op = db.Students.Where(s => s.StSchool_SchoolId == student.StSchool_SchoolId && s.Batch == student.Batch && s.Session == student.Session).ToList();

            Student st = new Student();
            DataTable dt = LinqToTable.LINQResultToDataTable(op);
            st.Batch = 1;
            st.Session = student.Session;
            st.StSchool_SchoolId = student.StSchool_SchoolId;
            ViewBag.optlist = dt;



            return View("Index", st);
        }

        // GET: Students/Delete/5
        [AutorizeUser]
        public ActionResult Delete(int? id)
        {
            Student student = db.Students.Find(id);
            if (student.Photo != null && student.Photo != "")
            {
                var a = student.Photo.Split('?');
                if (a[0] != "")
                {
                    string dirUrl = "/uploads/" + a[0];

                    //string dirUrl = "/uploads/" + student.Photo;
                    string dirPath = Server.MapPath(dirUrl);
                    if (System.IO.File.Exists(dirPath))
                    {
                        System.IO.File.Delete(dirPath);
                    }
                }
            }
            db.Students.Remove(student);
            db.SaveChanges();
            TempData["smsg"] = "success";
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.classlist = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectionName = new SelectList(sectionlist, "SectonName", "SectonName");
            //return RedirectToAction("Index", new { id = student.StSchool_SchoolId });
            Student st = new Student();
            st.StSchool_SchoolId = student.StSchool_SchoolId;
            st.Session = student.Session;
            st.Batch = student.Batch;
            return RedirectToAction("filterstudent", st);
        }

        // POST: Students/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[AutorizeUser]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Student student = db.Students.Find(id);
        //    db.Students.Remove(student);
        //    db.SaveChanges();
        //    TempData["smsg"] = "success";
        //    var classlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
        //    ViewBag.classlist = new SelectList(classlist, "ClassName", "ClassName");
        //    var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == student.StSchool_SchoolId && x.Session == student.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
        //    ViewBag.SectionName = new SelectList(sectionlist, "SectonName", "SectonName");
        //    return RedirectToAction("Index", new { id = student.StSchool_SchoolId });
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AutorizeUser]
        public ActionResult ImportExcel()
        {
            Student st = new Student();
            st.Batch = 1;
            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();
            int schoolid = (int)Session["schoolid"];
            var q = db.Schools.Where(x => x.SchoolId == schoolid).ToList();
            ViewBag.StSchool_SchoolId = new SelectList(q, "SchoolId", "SchoolName");
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == schoolid && x.Session == st.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.classlist = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == schoolid && x.Session == st.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectionName = new SelectList(sectionlist, "SectonName", "SectonName");
            return View(st);
           
        }
        [HttpPost]
        [AutorizeUser]
        public ActionResult ImportExcel(WebApplication1.Student st)
        {
            DateTime UpdatedOn = DateTime.Now;
            //var schoolid = db.Schools.Where(x => x.SchoolCode == st.schoolcode).Select(x => x.SchoolId);
            // custname = db.Schools.Where(x => x.SchoolId == Convert.ToInt32(schoolid)).Select(x => x.Customer);
            var photo = st.PhotoUpload.FileName; //custname.ToString() + st.schoolcode + st.PhotoUpload.FileName;
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
                    stud.AdmNo = (Convert.IsDBNull(dr["admno"]) ? "" : dr["admno"].ToString());
                    stud.rollno = (Convert.IsDBNull(dr["rollno"]) ? "" : dr["rollno"].ToString());


                    stud.AllCorrect = false;
                    stud.ClassName = (Convert.IsDBNull(dr["Class"]) ? "" : dr["Class"].ToString());
                    stud.Dob = (Convert.IsDBNull(dr["dob"]) ? "" : dr["dob"].ToString());
                    stud.FatherName = (Convert.IsDBNull(dr["Father"]) ? "" : dr["Father"].ToString());
                    stud.MotherName = (Convert.IsDBNull(dr["Mother"]) ? "" : dr["Mother"].ToString());
                    stud.Mobile = (Convert.IsDBNull(dr["Mobile"]) ? "" : dr["Mobile"].ToString());
                    stud.NeedUpdation = true;
                    stud.OptedConveyance = false;
                    stud.Res1 = (Convert.IsDBNull(dr["res1"]) ? "" : dr["res1"].ToString());
                    stud.Res2 = (Convert.IsDBNull(dr["res2"]) ? "" : dr["res2"].ToString());
                    stud.ReviewDate = DateTime.Today;
                    stud.UpdatedOn = DateTime.Today;
                    stud.StName = (Convert.IsDBNull(dr["StName"]) ? "" : dr["StName"].ToString());
                    stud.Session = st.Session;
                    stud.SectonName = (Convert.IsDBNull(dr["Section"]) ? "" : dr["Section"].ToString());
                    stud.StSchool_SchoolId = st.StSchool_SchoolId;
                    stud.ReviewedBySchool = false;
                    stud.IsPhotoUpload = false;
                    stud.Photo = "";
                    stud.Excel_Photo = (Convert.IsDBNull(dr["photo"]) ? "" : dr["photo"].ToString());
                    stud.Batch = st.Batch;
                    stud.house = (Convert.IsDBNull(dr["house"]) ? "" : dr["house"].ToString());
                    stud.driver = (Convert.IsDBNull(dr["driver_name"]) ? "" : dr["driver_name"].ToString());
                    stud.driver_mobile = (Convert.IsDBNull(dr["driver_mobile"]) ? "" : dr["driver_mobile"].ToString());
                    stud.conveyance = (Convert.IsDBNull(dr["conveyance"]) ? "" : dr["conveyance"].ToString());
                    stud.stop = (Convert.IsDBNull(dr["stop"]) ? "" : dr["stop"].ToString());
                    stud.designation = (Convert.IsDBNull(dr["designation"]) ? "" : dr["designation"].ToString());
                    if (dr["emergency_no"].ToString() == null || dr["emergency_no"].ToString() == "")
                    {
                        stud.emergency_no = 0;
                    }
                    else
                    {
                        stud.emergency_no = Convert.ToInt64(dr["emergency_no"]);
                    }

                    stud.bloodgroup = (Convert.IsDBNull(dr["bloodgroup"]) ? "" : dr["bloodgroup"].ToString());
                    db.Students.Add(stud);
                    db.SaveChanges();
                }

                TempData["smsg"] = "success";
            }
            st.Batch = 1;
            st.Session = db.session_master.Where(x => x.isactive == true).Select(x => x.session).FirstOrDefault();
            st.sessionlist = db.session_master.ToList();
            int schoolid = (int)Session["schoolid"];
            var q = db.Schools.Where(x => x.SchoolId == schoolid).ToList();
            ViewBag.StSchool_SchoolId = new SelectList(q, "SchoolId", "SchoolName");
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == schoolid && x.Session == st.Session).GroupBy(x => x.ClassName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.classlist = new SelectList(classlist, "ClassName", "ClassName");
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == schoolid && x.Session == st.Session).GroupBy(x => x.SectonName).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.SectionName = new SelectList(sectionlist, "SectonName", "SectonName");
            return View(st);
        }
        [AutorizeUser]
        public ActionResult school_changepassword()
        {
            Models.Class1.ChangePassword ch = new Models.Class1.ChangePassword();
            ch.id = (int)Session["schoolid"];
            return View(ch);
        }

        [HttpPost]
        [AutorizeUser]
        public ActionResult school_changepassword(Models.Class1.ChangePassword ch)
        {
            if (ch.id > 0)
            {
                if (ch.ConfirmPassword == ch.NewPassword)
                {
                    var q = db.Schools.Where(x => x.SchoolId == ch.id && x.password == ch.OldPassword).FirstOrDefault();
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

            }
            else
            {
                TempData["errormsg"] = "Select School..!";
                return View(ch);
            }
            return View(ch);
        }


    }
}
