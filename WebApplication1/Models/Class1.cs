using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Class1
    {
        public enum UserTypes
        {
            Admin = 1,
            User = 2,
            School = 3
        }


        public class studnetupdate
        {
            public int StudentId { get; set; }
            public string AdmNo { get; set; }
            public string StName { get; set; }
            public string FatherName { get; set; }
            public string MotherName { get; set; }
         
            public string Dob { get; set; }
            public string Mobile { get; set; }
            public string Address { get; set; }
            public bool OptedConveyance { get; set; }
            public string ClassName { get; set; }
            public string SectionName { get; set; }
            public string Session { get; set; }
            public string Photo { get; set; }
            public int batch { get; set; }

            public string Res1 { get; set; }
            public string Res2 { get; set; }
            public string conveyance { get; set; }
            public string stop { get; set; }
            public string driver { get; set; }
            public string driver_mobile { get; set; }
            public string designation { get; set; }
            public string rollno { get; set; }
            public string house { get; set; }
            public long emergency_no { get; set; }
            public string bloodgroup { get; set; }

            public Nullable<int> schoolid { get; set; }
            public HttpPostedFileBase croppedImage { get; set; }

            public HttpPostedFileBase PhotoUpload { get; set; }
        }



        public class TestCropImage
            {
            public HttpPostedFileBase croppedImage { get; set; }
            }

        public class studentjson
        {
            public int schoolid { get; set; }
            public string schoolname { get; set; }
        }


         public class ChangePassword
        {
            public int id { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


    }
}