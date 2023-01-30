// See https://aka.ms/new-console-template for more information
using HabrParser.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Xml;

Console.WriteLine("Hello, World!");


try
{
    //var url = $"https://habr.com/ru/post/{id}/";
    for (int i = 100000; i < 200000; i++)
    {

        var url = $"https://habr.com/ru/post/{i}/";

        var web = new HtmlWeb();
        var doc = web.Load(url);
        using (var fs = new FileStream("d:\\fileMainPrj.html", FileMode.Create))
        {
            doc.Save(fs);
        }

        //var jsonData = doc.DocumentNode.SelectNodes("script");// SelectSingleNode("window.__INITIAL_STATE__");
        foreach (var script in doc.DocumentNode.Descendants("script").ToArray())
        {
            //Console.BackgroundColor = ConsoleColor.DarkRed;
            //Console.WriteLine("-----------------------------------------------------------------------------");
            string s = script.InnerText;

            try
            {
                if (s.Substring(0, 25) == "window.__INITIAL_STATE__=")
                {
                    var jsonStr = s.Substring(25, s.Length - 147);
                    string strToReplace = $"\"articlesList\":{{\"{i}\"";
                    jsonStr = jsonStr.Replace(strToReplace, "\"articlesList\":{\"article\"");
                    Root data;
                    data = JsonConvert.DeserializeObject<Root>(jsonStr);
                    Console.ResetColor();

                    if(data.articlesList.articlesList.article != null)
                        Console.WriteLine(data.articlesList.articlesList.article.titleHtml);
                }

                // Modify s somehow
                HtmlTextNode text = (HtmlTextNode)script.ChildNodes
                                    .Single(d => d.NodeType == HtmlNodeType.Text);
                text.Text = s;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }
    }
}

catch (Exception e)
{
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.WriteLine(e.Message.PadRight(60));
    Console.ResetColor();
    //return article;
}

