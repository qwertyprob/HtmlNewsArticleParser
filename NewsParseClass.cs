using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ParserHtml; 

namespace ParserHtml.NewsParseClass
{
    public class NewsParseClass
    {
        //Properties
        public string Theme { get; set; } = "No theme";
        public string Title { get; set; } = "No title";
        public string ImageUrl { get; set; } = "No Image Url";
        public string AltText { get; set; } = "No Text from Img";
        public string PublishTime { get; set; } = "No Time";
        public DateTime ParseDate { get; set; } = DateTime.Now;
        public string Content { get; set; } = "No text";
        

        private string url = "https://zugo.md/";
        private string contentUrl = "https://zugo.md/toate-stirile"; 

        //Methods
        public string[] SetArrayOfPages(int counter)
        {
            string[] pages = new string[counter];

            for(int i = counter-1; i>=0; i--)
            {
                pages[i] = url + $"page/{i+1}";

            }

            return pages;
        }

        public string SetPage(int numberOfPage)
        {
            return url + $"page/{numberOfPage}";
        }
        //Search by theme, title, imageurl,alttext,time

        public List<NewsParseClass> GetNewsFromPages(string page)
        {
            var web = new HtmlWeb();
            var document = web.Load(page);

            List<NewsParseClass> classes = new List<NewsParseClass>();
            NewsParseClass classToJson = new NewsParseClass();
            //for Actualitatea 
            var pattern = document.DocumentNode.SelectNodes("/html/body/div[1]/div/div/div[3]/div/div/div/div/div[2]/div/div[2]/ul/li");
            if (pattern != null)
            {
                foreach (var item in pattern)
                {
                    try
                    {
                        //Theme
                        var selectArticle = item.SelectSingleNode("/html/body/div[1]/div/div/div[2]/div/div/div/div/div/div/div[1]/h3");
                        var decodeArticle = DecodeAndCorrect(HtmlEntity.DeEntitize(selectArticle.InnerHtml.Trim()).ToString());
                      
                        //Article
                        HtmlNode content = item.SelectSingleNode(".//div[2]/div/h2/a");
                        var decodeContent = content != null ? HtmlEntity.DeEntitize(content.InnerHtml.Trim()) : "Заголовок не найден";
                        //Img alt text    
                        HtmlNode contentAlt = item.SelectSingleNode(".//a/img");
                        var decodeContentAlt = contentAlt != null ? contentAlt.GetAttributeValue("alt", "Not found") : "Alt не найден";
                        //Img src text       
                        HtmlNode contentImg = item.SelectSingleNode(".//a/img");
                        var decodeContentImg = contentImg != null ? contentImg.GetAttributeValue("src", "Not Found") : "Изображение не найдено";
                        //Published time      
                        HtmlNode time = item.SelectSingleNode(".//div[2]/div/div[1]/span");
                        var decodeTime = time != null ? HtmlEntity.DeEntitize(time.InnerHtml.Trim()) : "No time";
                        //Text      
                        var decodeTextContent = ParseContent(decodeContent);
                        //Parsed time       
                        var timeParseMade = DateTime.Now;
                        
                        classToJson = new NewsParseClass()
                        {
                            Theme = decodeArticle,
                            Title = decodeContent,
                            ImageUrl = decodeContentImg,
                            AltText = decodeContentAlt,
                            PublishTime = decodeTime,
                            ParseDate = DateTime.Now,
                            Content = decodeTextContent,
                            
                        };
                        classes.Add(classToJson);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла ошибка: " + ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Статьи не найдены.");
            }

            // Запись в JSON файл
            WriteToJsonFile(classes);

            return classes;
        }
        private void WriteToJsonFile(List<NewsParseClass> classes)
        {
            
            string directoryPath = "C:\\Users\\eqspe\\source\\repos\\parser\\news\\";

            
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (var newsItem in classes)
            {
                
                string safeTitle = Regex.Replace(newsItem.Title, @"[^a-zA-Z0-9]", "_");

                
                string filePath = Path.Combine(directoryPath, $"{safeTitle}.json");

                
                string json = JsonConvert.SerializeObject(newsItem, Formatting.Indented);

                
                File.WriteAllText(filePath, json);
            }
        }

        private static string DecodeAndCorrect(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            
            string decodedString = HttpUtility.HtmlDecode(input);

           
            return decodedString.Replace('?', 'ș'); 
        }


        //Search by content(text)
        private  string ParseContent(string page)
        {
            page = SetUrl(page);
            var web = new HtmlWeb();
            var document = web.Load(page);


            HtmlNodeCollection content = document.DocumentNode.SelectNodes("//div[@class='entry-content entry clearfix xsingile']/p\r\n");

            string decodeContent = "";

            if (content != null)
            {
                foreach (var node in content)
                {
                   
                    decodeContent += HtmlEntity.DeEntitize(node.InnerHtml.Trim()) + " "; 
                }
            }
            else
            {
                decodeContent = page;
            }

            

            return decodeContent.Replace("<strong>","");

        }

        public string SetUrl(string parsedUrl)
        {
            parsedUrl = Regex.Replace(parsedUrl, "^-", ""); 
            parsedUrl = parsedUrl
                .Replace(' ', '-')
                .Replace('„', '\0')
                .Replace('”', '\0')
                .Replace(',', '\0')
                .Replace('.', '\0')
                .Replace('!', '\0')
                .Replace("ă", "a")
                .Replace("â", "a")
                .Replace("î", "i")
                .Replace("ș", "s")
                .Replace("ț", "t")
                .Replace("?", "s")
                .Replace(":", "")
                .Replace("|", "")
                .Replace("--", "-")
                .Replace("/-", "/")
                .Replace("foto", "")
                .Replace("video", "")
                .Replace("\u0000", "") 
                .Replace("\n", "") 
                .Replace("\r", "") 
                .Replace("intrun", "intr-un")


                .ToLower();
            return contentUrl + $"/{parsedUrl}";
        }
        public int CalculateTotalWordsInContent(List<NewsParseClass> newsList)
        {
            int totalWords = 0;

            foreach (var newsItem in newsList)
            {
                if (!string.IsNullOrEmpty(newsItem.Content))
                {
                    // Разделяем текст контента на слова по пробелам и подсчитываем их количество
                    var words = newsItem.Content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    totalWords += words.Length;
                }
            }

            return totalWords;
        }

        public double CalculateAverageWordsInContent(List<NewsParseClass> newsList)
        {
            int totalWords = CalculateTotalWordsInContent(newsList);
            return newsList.Count > 0 ? (double)totalWords / newsList.Count : 0;
        }


    }
}
