using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserHtml
{
    public class NewsParseClass
    {
        public string Theme { get; set; } = "No theme";
        public string Title { get; set; } = "No title";
        public string ImageUrl { get; set; } = "No Image Url";
        public string AltText { get; set; } = "No Text from Img";
        public string PublishTime { get; set; } = "No Time";
        public DateTime ParseDate { get; set; } = DateTime.Now;
        public string Content { get; set; } = "No text";

        public string url = "https://zugo.md/";
        public string[] SetArrayOfPages(int counter)
        {
            string[] pages = new string[counter];

            for(int i = counter-1; i>=0; i--)
            {
                pages[i] = url + $"page/{i+1}";

            }

            return pages;
        }

    }
}
