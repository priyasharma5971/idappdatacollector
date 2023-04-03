using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testEntityApi.Filter;

namespace WebApplication1.Controllers
{
    [AllowCrossSiteJsonAttribute]
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult upload(WebApplication1.Models.Class1.TestCropImage s)
        {
            if (s.croppedImage != null)
            {
                var msg = "";

                try
                {
                    var sizeinbyte = s.croppedImage.ContentLength;
                    var extension = Path.GetExtension(s.croppedImage.FileName);
                    var sizeinkb = (sizeinbyte) / (1024);
                    if (sizeinkb >  5120)
                    {
                        TempData["errormsg"] = "Photo Size should not be greator than 5 MB..!";
                        return Json(TempData["errormsg"], JsonRequestBehavior.AllowGet);
                    }
                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        //student.AdmNo + student.Session + schoolcode.SchoolCode + extension;

                        var ServerSavePath = Path.Combine(Server.MapPath("~/uploads/"), s.croppedImage.FileName);

                        s.croppedImage.SaveAs(ServerSavePath);

                        Image img = Image.FromFile(Server.MapPath("/uploads/" + s.croppedImage.FileName));
                        Graphics graphics = Graphics.FromImage(img);
                        Bitmap bmp = new Bitmap(390, 480); //1.3x1.6 inch 300 dpi image
                        bmp.SetResolution(300, 300);
                        Graphics g = Graphics.FromImage(bmp);
                        g.DrawImage(img, new Rectangle(0, 0, 390, 480), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                        bmp.Save(Server.MapPath("/uploads/" + s.croppedImage.FileName+"_resized.jpg"));
                        return Json("success", JsonRequestBehavior.AllowGet);
                        //m.Photo = s.croppedImage.FileName;
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
                }

            }

            return View();
        }
    }
}