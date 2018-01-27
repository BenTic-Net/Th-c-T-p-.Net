using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CarDealer.Areas.Client.Models
{
    public class BlogViewModel
    {
        public long NewId { get; set; }
        public string Title { get; set; }
        public int? Commentcount { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> TopHot { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }       
        public string CreatedBy { get; set; }        
        public string Topic { get; set; }
        public int Waranty { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }
        public List<string> Topics { get { if (Topic.Contains(',')) { return Topic.Split(',').ToList(); } return new List<string> { Topic }; } }

        public IEnumerable<string> SubContent { get { return Regex.Replace(Content, "<.*?>", String.Empty).Split(' ').ToList().Take(70); } }
    }
}