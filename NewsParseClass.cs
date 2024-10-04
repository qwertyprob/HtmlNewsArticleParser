using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserHtml
{
    public class NewsParseClass
    {
        public string Theme { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public string PublishTime { get; set; }
        public DateTime ParseDate { get; set; }
        public string Content { get; set; }
    }
}
