using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Activities;

namespace WebApplication1.Models
{
    public class BulkImage
    {
        //[File(FileTypes = new string[])]
        public Nullable<int> StSchool_SchoolId { get; set; }
        public string session { get; set; }
        public IEnumerable Files { get; set; }
        public virtual School School { get; set; }
        public List<session_master> sessionlist { get; set; }
    }

    public class bulhimagetest
    {
        public Nullable<int> StSchool_SchoolId { get; set; }
        public string session { get; set; }
        public IEnumerable Files { get; set; }
        public string filename { get; set; }

    }
}
