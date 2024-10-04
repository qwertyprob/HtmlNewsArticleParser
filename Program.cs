using System;
using HtmlAgilityPack;
using Newtonsoft.Json; 
using System.IO;
using ParserHtml;
using System.Text;

var jsonArray = new List<NewsParseClass>();

var jsonElemList = BigMehod.BigMethodRealization("https://zugo.md/");

var jsonElemListSecondPage = BigMehod.BigMethodRealization("https://zugo.md/page/2");

var jsonElemListThirdPage = BigMehod.BigMethodRealization("https://zugo.md/page/3");

    foreach (var item in jsonElemList)
    {
       jsonArray.Add(item);
    }
    foreach (var item in jsonElemListSecondPage)
    {
        jsonArray.Add(item);
    }
    foreach (var item in jsonElemListThirdPage)
    {
        jsonArray.Add(item);
    }


//JsonParse 

string json = JsonConvert.SerializeObject(jsonElemList, Formatting.Indented);
Console.WriteLine(json);

using (var fs = new FileStream("C:\\Users\\eqspe\\source\\repos\\parser\\news.json", FileMode.Append, FileAccess.Write))
{
    // Convert the JSON string to a byte array
    byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

    // Optionally add a newline before appending new data
    byte[] newlineBytes = System.Text.Encoding.UTF8.GetBytes(Environment.NewLine);
    fs.Write(newlineBytes, 0, newlineBytes.Length); // Write newline to separate entries

    // Write the byte array to the file
    fs.Write(jsonBytes, 0, jsonBytes.Length);
}




