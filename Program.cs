//MAIN FILE!!!!!


using ParserHtml;

var pe = new NewsParseClass();


var arr=pe.SetArrayOfPages(3);


foreach(var item in arr)
{
    Console.WriteLine(item);
}

