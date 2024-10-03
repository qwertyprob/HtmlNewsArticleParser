// Site url: https://ru.diez.md/ on russian 
//Html parser into json + streamwrite(make it fiele) 


using HtmlAgilityPack;
using static System.Net.WebRequestMethods;



string url = @"https://tv8.md/ru";

var web = new HtmlWeb();

var document = web.Load(url);

HtmlNode titleDiv = document.DocumentNode.SelectSingleNode("/html/body/div[@class='MuiContainer-root layout-body-container jss12 MuiContainer-disableGutters MuiContainer-maxWidthXl']/div[@class='MuiContainer-root jss66 jss77 MuiContainer-maxWidthLg']/div[@class='MuiGrid-root jss65 MuiGrid-container MuiGrid-spacing-xs-3']/div/div[@class='MuiGrid-root MuiGrid-item MuiGrid-grid-sm-12 MuiGrid-grid-md-6']/a/div[@class='jss139']/div");
var decode = HtmlEntity.DeEntitize(titleDiv.InnerHtml);
Console.WriteLine(decode);


