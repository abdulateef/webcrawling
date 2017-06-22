using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace CrawlerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            startCrawlerasync();
            Console.ReadLine();
            
        }

        private static async Task startCrawlerasync()
        {
           
            //the url of the page we want to test
            var url = "http://www.automobile.tn/neuf/bmw.3/";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // a list to add all the list of cars and the various prices 
            var cars = new List<Car>();
            var divs =
            htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("article_new_car article_last_modele")).ToList();
                       
            foreach(var div in divs)
            {

                var car = new Car
                {
                     
                    Model = div.Descendants("h2").FirstOrDefault().InnerText,
                    Price = div.Descendants("div").FirstOrDefault().InnerText,
                    Link = div.Descendants("a").FirstOrDefault().ChildAttributes("href").FirstOrDefault().Value,
                    ImageUrl = div.Descendants("img").FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value
                };
                
                cars.Add(car);              
            }
            // Connection string 
            string MyConnection = "DRIVER={MySQL ODBC 3.51 Driver};Server=localhost;Database=crawlerdemo;User Id=root;Password=";
            //string MyConnection = "datasource=localhost;username=root;password=";  
            OdbcConnection con = new OdbcConnection(MyConnection);
            con.Open();

            try
            {
                int count = cars.Count;
                foreach(var item in cars)
                {
                    for(int i = 0; i < count; i++)
                    {
                        string query = "insert into carinfor(Model,Price,Link,ImageUrl) value(?,?,?,?);";
                        OdbcCommand cmd = new OdbcCommand(query, con);
                        cmd.Parameters.Add("?Model", OdbcType.VarChar).Value = cars[i].Model;
                        cmd.Parameters.Add("?Price", OdbcType.VarChar).Value = cars[i].Price;
                        cmd.Parameters.Add("?Link", OdbcType.VarChar).Value = cars[i].Link;
                        cmd.Parameters.Add("?ImageUrl", OdbcType.VarChar).Value = cars[i].ImageUrl;
                        OdbcDataReader reader = cmd.ExecuteReader();
                        reader.Close();
                    }

                    count = 0;
                }
            }
            catch(Exception ex)
            {

                Console.WriteLine(ex.Message);
            }  
           
            con.Close();
            Console.WriteLine("Successful....");
            Console.WriteLine("Press Enter to exit the program...");
            ConsoleKeyInfo keyinfor = Console.ReadKey(true);
            if(keyinfor.Key == ConsoleKey.Enter)
            {
                System.Environment.Exit(0);
            }

        }
       
    }
}
