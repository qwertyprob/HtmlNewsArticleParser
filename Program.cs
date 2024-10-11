using ParserHtml;
using System.Collections.Generic;

var MainClass = new NewsParseClass();

Console.OutputEncoding = System.Text.Encoding.UTF8;

var totalPages = 20;
var allParsedNews = new List<NewsParseClass>(); 

for (int i = 1; i <= totalPages; i++)
{
    var page = MainClass.SetPage(i);
    var parsedNews = MainClass.GetNewsFromPages(page);
    allParsedNews.AddRange(parsedNews);
}


Console.WriteLine("Парсинг завершён. Найденные новости:");
foreach (var news in allParsedNews)
{
    Console.WriteLine($"Theme:\t {news.Theme}");
    Console.WriteLine($"Title:\t {news.Title}");
    Console.WriteLine($"ImgUrl:\t {news.ImageUrl} (alt: {news.AltText})");
    Console.WriteLine($"Publish Time: \t {news.PublishTime}");
    Console.WriteLine($"Date now: \t {news.ParseDate}");
    Console.WriteLine($"Text: {news.Content}");
    Console.WriteLine(new string('-', 40)); // Разделитель между новостями
}

Console.WriteLine("Нажмите любую клавишу для выхода...");
Console.ReadKey();
