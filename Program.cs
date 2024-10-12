using ParserHtml;
using ParserHtml.NewsParseClass;
using System.Collections.Generic;

var mainClass = new NewsParseClass();

Console.OutputEncoding = System.Text.Encoding.UTF8;

var totalPages = 20;
var allParsedNews = new List<NewsParseClass>();

for (int i = 1; i <= totalPages; i++)
{
    var page = mainClass.SetPage(i);
    var parsedNews = mainClass.GetNewsFromPages(page);
    allParsedNews.AddRange(parsedNews);
}

Console.WriteLine("Парсинг завершён. Найденные новости:");

var totalWordsCount = CalculateTotalWordsInContent(allParsedNews);
var averageWordsCount = CalculateAverageWordsInContent(allParsedNews);
var corpses = SplitIntoCorpses(allParsedNews, 5); // делим на корпуса по 5 статей

Console.WriteLine($"Общее количество слов: {totalWordsCount}");
Console.WriteLine($"Среднее количество слов в статьях: {averageWordsCount}");

Console.WriteLine("Количество слов в каждом корпусе:");
foreach (var corpse in corpses)
{
    var corpseWordsCount = CalculateTotalWordsInContent(corpse);
    Console.WriteLine($"Количество слов в корпусе: {corpseWordsCount}");
}

Console.WriteLine("Нажмите любую клавишу для выхода...");
Console.ReadKey();

// Методы для подсчета слов
int CalculateTotalWordsInContent(List<NewsParseClass> newsList)
{
    int totalWords = 0;

    foreach (var news in newsList)
    {
        totalWords += news.Content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    return totalWords;
}

double CalculateAverageWordsInContent(List<NewsParseClass> newsList)
{
    if (newsList.Count == 0) return 0;

    var totalWords = CalculateTotalWordsInContent(newsList);
    return (double)totalWords / newsList.Count;
}

// Метод для разбивки на корпуса
List<List<NewsParseClass>> SplitIntoCorpses(List<NewsParseClass> newsList, int size)
{
    var corpses = new List<List<NewsParseClass>>();

    for (int i = 0; i < newsList.Count; i += size)
    {
        corpses.Add(newsList.Skip(i).Take(size).ToList());
    }

    return corpses;
}
