using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserHtml
{
    public class BigMehod
    {

        public static List<NewsParseClass> BigMethodRealization(string url)
        {
            
            var web = new HtmlWeb();
            var document = web.Load(url);


            List<NewsParseClass> classes = new List<NewsParseClass>();

            NewsParseClass classToJson = new NewsParseClass();


            //for Actualitatea 
            var articles = document.DocumentNode.SelectNodes("/html/body/div[1]/div/div/div[3]/div/div/div/div/div[2]/div/div[2]/ul/li");

            if (articles != null)
            {
                foreach (var article in articles)
                {
                    try
                    {
                        //Theme
                        var selectArticle = document.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[3]/div/div/div/div/div[2]/div/div[1]/h3");
                        var decodeArticle = HtmlEntity.DeEntitize(selectArticle.InnerHtml.Trim());

                        //Article
                        HtmlNode content = article.SelectSingleNode(".//div[2]/div/h2/a");
                        var decodeContent = content != null ? HtmlEntity.DeEntitize(content.InnerHtml.Trim()) : "Заголовок не найден";


                        //Img alt text    
                        HtmlNode contentAlt = article.SelectSingleNode(".//a/img");
                        var decodeContentAlt = contentAlt != null ? contentAlt.GetAttributeValue("alt", "Not found") : "Alt не найден";


                        //Img src text       
                        HtmlNode contentImg = article.SelectSingleNode(".//a/img");
                        var decodeContentImg = contentImg != null ? contentImg.GetAttributeValue("src", "Not Found") : "Изображение не найдено";


                        //Published time      
                        HtmlNode time = article.SelectSingleNode(".//div[2]/div/div[1]/span");
                        var decodeTime = time != null ? HtmlEntity.DeEntitize(time.InnerHtml.Trim()) : "Время не найдено";


                        //Text      
                        HtmlNode textContent = article.SelectSingleNode(".//div/p");
                        var decodeTextContent = textContent != null ? HtmlEntity.DeEntitize(textContent.InnerHtml.Trim()) : "Text was not found";


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



            

            return classes; 
        }
    }
}
