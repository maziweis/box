using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FzBox.Web.Models
{
    public class VM_Book
    {
        public string EditionName { get; set; }

        public string BookName { get; set; }

        public string Cover { get; set; }

        public string Url { get; set; }

        public List<VM_Book_App> Apps { get; set; }

        public int? Subject { get; set; }

        public int? Edition { get; set; }

        public string CourseKey { get; set; }
    }

    public class VM_Book_App
    {
        public string AppType { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}