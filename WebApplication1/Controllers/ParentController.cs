using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testEntityApi.Filter;

namespace WebApplication1.Controllers
{
    [AllowCrossSiteJsonAttribute]
    public class ParentController : Controller
    {
        private IdCardDataManagerEntities1 db = new IdCardDataManagerEntities1();

        // GET: Parent

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password, string utype)
        {
            if (utype.ToLower() == "parent")
            {
                var parentcheck = db.ParentLogins.Where(x => x.username == username && x.upassword == password).FirstOrDefault();
                if (parentcheck != null)
                {
                   
                    if (parentcheck.isactive)
                    {
                        var a = db.Schools.Where(x => x.SchoolId == parentcheck.schoolid).FirstOrDefault();
                        Models.Class1.studentjson s = new Models.Class1.studentjson();
                        s.schoolid = a.SchoolId;
                        s.schoolname = a.SchoolName;
                        return Json(s, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }


                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var query = db.Schools.Where(m => m.ContactNo == username && m.password == password).FirstOrDefault();
                if (query != null)
                {
                    

                    if (query.IsActive)
                    {

                        if (query.Customer.ValidTill >= DateTime.Today)
                        {
                            var a = db.Schools.Where(x => x.SchoolId == query.SchoolId).FirstOrDefault();
                            Models.Class1.studentjson s = new Models.Class1.studentjson();
                            s.schoolid = a.SchoolId;
                            s.schoolname = a.SchoolName;
                            return Json(s, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            // TempData["errormsg"] = "You are not valid for Access this..!";
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

        }






        public ActionResult Index()
        {
            return View();
        }

        // GET: Parent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Parent/Create

        public ActionResult Create()
        {


            return View();
        }

        // POST: Parent/Create
        [HttpPost]
        public ActionResult FilterStudent(string admno, int schoolid)
        {
            var tillyear = Convert.ToInt32(DateTime.Now.Year) + Convert.ToInt32(1);
            var session = DateTime.Now.Year + "-" + tillyear;
            var stdata = db.Students.Where(x => x.AdmNo == admno && x.StSchool_SchoolId == schoolid).OrderByDescending(x => x.StudentId).FirstOrDefault();
            Student st = new Student();
            if (stdata != null)
            {

                st.StName = stdata.StName;
                st.Address = stdata.Address;
                st.AdmNo = stdata.AdmNo;
                st.FatherName = stdata.FatherName;
                st.MotherName = stdata.MotherName;
                st.Address = stdata.Address;
                st.Mobile = stdata.Mobile;
                st.Dob = stdata.Dob;
                st.ClassName = stdata.ClassName;
                st.SectonName = stdata.SectonName;
                st.Session = stdata.Session;
                st.OptedConveyance = stdata.OptedConveyance;
                st.StudentId = stdata.StudentId;
                st.Photo = stdata.Photo + "?dummy=" + DateTime.Now + ""; ; ;
                st.StSchool_SchoolId = schoolid;
                st.Res1 = stdata.Res1;
                st.Res2 = stdata.Res2;
                st.conveyance = stdata.conveyance;
                st.stop = stdata.stop;
                st.driver = stdata.driver;
                st.driver_mobile = stdata.driver_mobile;
                st.designation = stdata.designation;
                st.rollno = stdata.rollno;
                st.house = stdata.house;
                st.emergency_no = stdata.emergency_no;
                st.bloodgroup = stdata.bloodgroup;
            }
            else
            {
                st.StName = "";
                st.Address = "";
                st.FatherName = "";
                st.MotherName = "";
                st.Address = "";
                st.Mobile = "";
                st.Dob = DateTime.Today.ToShortDateString();
                st.ClassName = "";
                st.SectonName = "";
                st.Session = Convert.ToString(DateTime.Now.Year + "-" + (Convert.ToInt32(DateTime.Now.Year) + Convert.ToInt32(1)));
                st.OptedConveyance = false;
                st.StudentId = 0;
                st.Photo = "";
                st.AdmNo = admno;
                st.StSchool_SchoolId = schoolid;
                st.Res1 = "";

                st.Res2 = "";
                st.conveyance = "";
                st.stop = "";
                st.driver = "";
                st.driver_mobile = "";
                st.designation = "";
                st.rollno = "" ;
                st.house = "";
                st.emergency_no =0;
                st.bloodgroup = "";

            }
            return Json(st, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public ActionResult GetStudent(int studentid)
        {
            var tillyear = Convert.ToInt32(DateTime.Now.Year) + Convert.ToInt32(1);
            var session = DateTime.Now.Year + "-" + tillyear;
            var stdata = db.Students.Where(x => x.StudentId == studentid).FirstOrDefault();
            Student st = new Student();
            if (stdata != null)
            {

                st.StName = stdata.StName;
                st.Address = stdata.Address;
                st.AdmNo = stdata.AdmNo;
                st.FatherName = stdata.FatherName;
                st.MotherName = stdata.MotherName;
                st.Address = stdata.Address;
                st.Mobile = stdata.Mobile;
                st.Dob = stdata.Dob;
                st.ClassName = stdata.ClassName;
                st.SectonName = stdata.SectonName;
                st.Session = stdata.Session;
                st.OptedConveyance = stdata.OptedConveyance;
                st.StudentId = stdata.StudentId;
                st.Photo = stdata.Photo + "?dummy=" + DateTime.Now + "";
                st.Batch = stdata.Batch;
                st.StSchool_SchoolId = stdata.StSchool_SchoolId;
                st.Res1 = stdata.Res1;

                st.Res2 = stdata.Res2;
                st.conveyance = stdata.conveyance;
                st.stop = stdata.stop;
                st.driver = stdata.driver;
                st.driver_mobile = stdata.driver_mobile;
                st.designation = stdata.designation;
                st.rollno = stdata.rollno;
                st.house = stdata.house;
                st.emergency_no = stdata.emergency_no;
                st.bloodgroup = stdata.bloodgroup;
            }


            return Json(st, JsonRequestBehavior.AllowGet);


        }



        [HttpPost]
        public ActionResult FilterStudent_School(int schoolid, string sess, string classname, string sectionname, int batch)
        {

            var session = sess;
            var stdata = db.Students.Where(x => x.StSchool_SchoolId == schoolid && x.Session == sess && x.Batch == batch).ToList();
            if (classname != "")
            {
                stdata = stdata.Where(x => x.ClassName == classname).ToList();
            }
            if (sectionname != "")
            {
                stdata = stdata.Where(x => x.SectonName == sectionname).ToList();
            }
            List<Student> st = new List<Student>();
            if (stdata != null)
            {
                foreach (var item in stdata)
                {
                    Student st1 = new Student();
                    st1.StName = item.StName;
                    st1.Address = item.Address;
                    st1.AdmNo = item.AdmNo;
                    st1.FatherName = item.FatherName;
                    st1.MotherName = item.MotherName;
                    st1.Address = item.Address;
                    st1.Mobile = item.Mobile;
                    st1.Dob = item.Dob;
                    st1.ClassName = item.ClassName;
                    st1.SectonName = item.SectonName;
                    st1.Session = item.Session;
                    st1.OptedConveyance = item.OptedConveyance;
                    st1.StudentId = item.StudentId;
                    st1.Photo = item.Photo + "?dummy=" + DateTime.Now + "";
                    st1.StSchool_SchoolId = schoolid;
                    st1.Res1 = item.Res1;
                    st1.Res2 = item.Res2;
                    st1.conveyance = item.conveyance;
                    st1.stop = item.stop;
                    st1.driver = item.driver;
                    st1.driver_mobile = item.driver_mobile;
                    st1.designation = item.designation;
                    st1.rollno = item.rollno;
                    st1.house = item.house;
                    st1.emergency_no = item.emergency_no;
                    st1.bloodgroup = item.bloodgroup;
                    st.Add(st1);
                }
            }

            return Json(st.OrderByDescending(x => x.StudentId).ToList(), JsonRequestBehavior.AllowGet);


        }




        [HttpPost]
        public ActionResult SaveUpdateRecord(WebApplication1.Models.Class1.studnetupdate m)
        {

            var stdata = db.Students.Where(x => x.StudentId == m.StudentId && x.StSchool_SchoolId == m.schoolid && x.Session == m.Session).FirstOrDefault();



            if (m.croppedImage != null)
            {
                var msg = "";

                try
                {
                    var sizeinbyte = m.croppedImage.ContentLength;
                    var extension = ".jpg"; //Path.GetExtension(m.croppedImage.ContentType.Substring(5,3));
                    var sizeinkb = (sizeinbyte) / (1024);


                    var schoolcode = db.Schools.Where(x => x.SchoolId == m.schoolid).FirstOrDefault();
                    var filename = "";
                    if (m.StudentId > 0)
                    {

                        filename = m.StudentId + m.Session + schoolcode.SchoolCode + extension;
                    }
                    else
                    {
                        int maxstid = db.Students.ToList().Select(x => x.StudentId).Max();
                        filename = maxstid + 1 + m.Session + schoolcode.SchoolCode + extension;
                    }


                    Image img = Image.FromStream(m.croppedImage.InputStream);
                    Graphics graphics = Graphics.FromImage(img);
                    Bitmap bmp = new Bitmap(390, 480, System.Drawing.Imaging.PixelFormat.Format24bppRgb); //1.3x1.6 inch 300 dpi image
                    bmp.SetResolution(300, 300);
                    Graphics g = Graphics.FromImage(bmp);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(img, new Rectangle(0, 0, 390, 480), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                    //save image as jpg with full quality
                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                    System.Drawing.Imaging.Encoder myEncoder =
                            System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    bmp.Save(Server.MapPath("/uploads/" + filename), jgpEncoder, myEncoderParameters);
                    img.Dispose();
                    m.Photo = filename;

                }
                catch (Exception e)
                {
                    TempData["errormsg"] = e.Message;
                    return Json(TempData["errormsg"], JsonRequestBehavior.AllowGet);
                }

            }

            if(m.batch<1)
            {
                m.batch = 1;
            }


            if (stdata != null)
            {
                stdata.StName = m.StName;
                stdata.Address = m.Address;
                stdata.AdmNo = m.AdmNo;
                stdata.FatherName = m.FatherName;
                stdata.MotherName = m.MotherName;
                stdata.Address = m.Address;
                stdata.Mobile = m.Mobile;
                stdata.Dob = m.Dob;
                stdata.ClassName = m.ClassName;
                stdata.SectonName = m.SectionName;
                stdata.Session = m.Session;
                stdata.OptedConveyance = m.OptedConveyance;
                stdata.Res1 = m.Res1;
                stdata.Res2 = m.Res2;
                stdata.conveyance = m.conveyance;
                stdata.stop = m.stop;
                stdata.driver = m.driver;
                stdata.driver_mobile = m.driver_mobile;
                stdata.designation = m.designation;
                stdata.rollno = m.rollno;
                stdata.Batch = m.batch;
                stdata.house = m.house;
                stdata.emergency_no =m.emergency_no;
                stdata.bloodgroup = m.bloodgroup;
                stdata.Photo = m.Photo;
                stdata.UpdatedOn = DateTime.Now;
                if (m.Photo != null)
                {
                    stdata.IsPhotoUpload = true;
                }
                try
                {
                    db.Entry(stdata).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }

                catch (Exception ex)
                {
                    TempData["errormsg"] = ex.Message;
                    return Json(TempData["errormsg"], JsonRequestBehavior.AllowGet);

                }


            }
            else
            {

                Student st = new Student();
                st.StName = m.StName;
                st.Address = m.Address;
                st.AdmNo = m.AdmNo;
                st.FatherName = m.FatherName;
                st.MotherName = m.MotherName;
                st.Mobile = m.Mobile;
                st.Dob = m.Dob;
                st.ClassName = m.ClassName;
                st.SectonName = m.SectionName;
                st.Session = m.Session;
                st.OptedConveyance = m.OptedConveyance;
                st.Photo = m.Photo;
                st.StSchool_SchoolId = m.schoolid;
                st.Res1 = m.Res1;
                st.Res2 = m.Res2;
                st.Batch = m.batch;
                st.conveyance = m.conveyance;
                st.stop = m.stop;
                st.driver = m.driver;
                st.driver_mobile = m.driver_mobile;
                st.designation = m.designation;
                st.rollno = m.rollno;
                st.house = m.house;
                st.emergency_no = m.emergency_no;
                st.bloodgroup = m.bloodgroup;
                st.UpdatedOn = DateTime.Now;
                if (m.Photo != null)
                {
                    st.IsPhotoUpload = true;
                }
                try
                {
                    db.Students.Add(st);
                    db.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex1)
                {
                    TempData["errormsg"] = ex1.Message;
                    return Json(TempData["errormsg"], JsonRequestBehavior.AllowGet);

                }

            }

        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult SelectField(int? schoolid)
        {
            List<softwareoption> op = db.softwareoptions.Where(x => x.StSchool_SchoolId == schoolid).ToList();

            return Json(op[0], JsonRequestBehavior.AllowGet);
        }

        public bool isfileformatisvalid(string filename, byte[] FileBytes)
        {
            string fileextension = new FileInfo(filename).Extension.ToUpper();

            if (FileBytes == null || FileBytes.Length == 0)
            {
                return false;
            }

            if (FileBytes.Take(7).SequenceEqual(PDF) && fileextension == ".PDF")
            {
                return true;
            }
            else if (FileBytes.Take(3).SequenceEqual(JPG) && (fileextension == ".JPG" || fileextension == ".JPEG"))
            {
                return true;
            }
            else if (FileBytes.Take(16).SequenceEqual(PNG) && fileextension == ".PNG")
            {
                return true;
            }
            else if (FileBytes.Take(16).SequenceEqual(DOCX) && fileextension == ".DOCX")
            {
                return true;
            }

            else if (FileBytes.Take(4).SequenceEqual(GIF) && fileextension == ".GIF")
            {
                return true;
            }

            else
            {
                return false;
            }

        }


        private static readonly byte[] JPG = { 255, 216, 255 };
        private static readonly byte[] PDF = { 37, 80, 68, 70, 45, 49, 46 };
        private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
        private static readonly byte[] DOCX = { 80, 75, 3, 4 };
        private static readonly byte[] GIF = { 71, 73, 70, 56 };






        // GET: Parent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Parent/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Parent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Parent/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult DeleteStudent(int id)
        {
            try
            {
                var q = db.Students.Where(x => x.StudentId == id).FirstOrDefault();
                db.Students.Remove(q);
                db.SaveChanges();
                return Json("Delete Success", JsonRequestBehavior.AllowGet);


            }
            catch
            {
                return Json("Not Delete", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult getclass(int schoolid, string session)
        {
            var classlist = db.Students.Where(x => x.StSchool_SchoolId == schoolid).Select(x => new { x.ClassName }).Distinct().ToList();
            //ViewBag.ClassName = new SelectList(classlist, "ClassName", "ClassName");
            return Json(classlist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult getsection(int schoolid, string session)
        {
            var sectionlist = db.Students.Where(x => x.StSchool_SchoolId == schoolid).Select(x => new { x.SectonName }).Distinct().ToList();

            return Json(sectionlist, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public ActionResult changepassword(int id, string npass, string cpass, string opass)
        {
            if (id > 0)
            {
                if (cpass == npass)
                {
                    var q = db.Schools.Where(x => x.SchoolId == id && x.password == opass).FirstOrDefault();
                    if (q != null)
                    {
                        q.password = npass;
                        db.Entry(q).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json("Success", JsonRequestBehavior.AllowGet);
                        //TempData["smsg"] = "Change Success";

                    }
                    else
                    {
                        return Json("Old Password is Wrong..!", JsonRequestBehavior.AllowGet);


                    }

                }
                else
                {
                    return Json("Confirm Password and New Password Not Correct..!", JsonRequestBehavior.AllowGet);


                }

            }
            else
            {
                return Json("Select School..!", JsonRequestBehavior.AllowGet);


            }

        }

        public ActionResult SessionList()
        {
            Student st = new Student();
           
            st.sessionlist = db.session_master.ToList();
            return Json(st, JsonRequestBehavior.AllowGet);
        }

    }
}
