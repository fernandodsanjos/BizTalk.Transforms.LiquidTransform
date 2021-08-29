using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;

using BizTalk.Transforms.LiquidTransform;
using System.Reflection;
namespace TestLiquid
{
    class Program
    {
        static void Main(string[] args)
        {
            //LiquidTests();

            //ObjectToXmlParser
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Start memory: {GC.GetTotalMemory(true)}");

            

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //TestMap();

           

            stopWatch.Stop();
            Console.WriteLine($"Elapsed: {stopWatch.ElapsedMilliseconds}");
            Console.WriteLine($"After: {GC.GetTotalMemory(false) / 1024}");
          
           
            Console.ReadKey();
        }

        static void TestMap()
        {
            LiquidTransform transform = new LiquidTransform();
            //transform.RegisterExtension
            transform.Load(File.ReadAllText(@"C:\repos\BizTalk.Transforms.LiquidTransform\BtsTestLiquid\FromXml\PersonToJson.liquid"));
            FileStream input = new FileStream(@"C:\repos\BizTalk.Transforms.LiquidTransform\BtsTestLiquid\Samples\person.xml", FileMode.Open);
            MemoryStream output = new MemoryStream();
            transform.Transform(input, null, output);

            StreamReader reader = new StreamReader(output);
            Console.WriteLine(reader.ReadToEnd());

        }
        static string TestXmlToObjectParser()
        {
            var obj = XmlToObjectParser.ParseFromXml(File.ReadAllText(@"C:\repos\BizTalk.Transforms.LiquidTransform\TestLiquid\generator1000.xml"));

            string template = "Order: {{ root.row[699].email }}";
            var liquidTemplate = DotLiquid.Template.Parse(template);

            Hash hash = Hash.FromDictionary(obj);
            return liquidTemplate.Render(hash);
        }
        
            static string TestXmlToDictionary()
        {
            var dic = XmlToDictionary.Parse(File.ReadAllText(@"C:\repos\BizTalk.Transforms.LiquidTransform\TestLiquid\generator1000.xml"));
            /*
            var template = @" {% for row in root.row %}
            {{ row.id }}
            {% endfor %}";
            */

            var template = @" {{ root.row[699].id }}";

            //var template = "Order: {{ root.row[699].id }}";
            var liquidTemplate = DotLiquid.Template.Parse(template);

            var hash = Hash.FromDictionary(dic);

            return liquidTemplate.Render(hash);
        }
       
       

        static void LiquidTestFilter()
        {
            
                var template = File.ReadAllText(@"C:\repos\BizTalk.Transforms.LiquidTransform\BtsTestLiquid\Map1.liquid");

                BizTalk.Transforms.LiquidTransform.LiquidTransform liquid = new BizTalk.Transforms.LiquidTransform.LiquidTransform();

                liquid.Load(template);

                FileStream input = new FileStream(@"C:\repos\BizTalk.Transforms.LiquidTransform\Testfiles\person.json", FileMode.Open);
                MemoryStream output = new MemoryStream();
                liquid.RegisterExtension("", "BizTalk.Transforms.LiquidTransform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=969e815b781bd674", "BizTalk.Transforms.LiquidTransform.LookupFilter");

                liquid.Transform(input, null, output);
           
                StreamReader outReder = new StreamReader(output);
                Console.WriteLine(outReder.ReadToEnd());

        }
        static void LiquidTests()
        {
            var template = File.ReadAllText(@"C:\repos\BizTalk.Transforms.LiquidTransform\BtsTestLiquid\Map1.liquid");

            BizTalk.Transforms.LiquidTransform.LiquidTransform liquid = new BizTalk.Transforms.LiquidTransform.LiquidTransform();

            liquid.Load(template);

            FileStream input = new FileStream(@"C:\repos\BizTalk.Transforms.LiquidTransform\Testfiles\person.json", FileMode.Open);
            MemoryStream output = new MemoryStream();
            liquid.Transform(input, null, output);

            StreamReader outReder = new StreamReader(output);
            Console.WriteLine(outReder.ReadToEnd());


            liquid = new BizTalk.Transforms.LiquidTransform.LiquidTransform();

            liquid.Load(@"{ 
State Is: {{items[0]}} 
}");

            MemoryStream mem = new MemoryStream();
            byte[] bts = UTF8Encoding.UTF8.GetBytes("[\"Agda\", \"Emilio\"]");
            mem.Write(bts, 0, bts.Length);
            mem.Position = 0;


            output = new MemoryStream();
            liquid.Transform(mem, null, output);

            outReder = new StreamReader(output);
            Console.WriteLine(outReder.ReadToEnd());


            template = File.ReadAllText(@"C:\repos\BizTalk.Transforms.LiquidTransform\BtsTestLiquid\xmltojson.liquid");

            liquid = new BizTalk.Transforms.LiquidTransform.LiquidTransform();

            liquid.Load(template);

            input = new FileStream(@"C:\repos\BizTalk.Transforms.LiquidTransform\BtsTestLiquid\persons.xml", FileMode.Open);
            output = new MemoryStream();
            liquid.Transform(input, null, output);

            outReder = new StreamReader(output);
            Console.WriteLine(outReder.ReadToEnd());

            var body = @"<Order><OrderId>1001</OrderId><OrderLine>
<RowNo>1</RowNo>
    <ArticleNo>2301</ArticleNo>
  </OrderLine>
  <OrderLine>
    <RowNo>2</RowNo>
    <ArticleNo>2302</ArticleNo>
  </OrderLine>
</Order>";
            var order = XmlToObjectParser.ParseFromXml(body);


            template = "Order: {{ Order.OrderId }}";
            var liquidTemplate = DotLiquid.Template.Parse(template);

            Hash hash = Hash.FromDictionary(order);
            Console.WriteLine(liquidTemplate.Render(hash));

            /*
             MemoryStream mem = new MemoryStream(1024);
             int tp = 0;
             byte[] blank = new byte[] { 0, 0, 0, 0 };
             mem.Write(blank, 0, blank.Length);


             byte[] bts = UTF8Encoding.UTF8.GetBytes("  <");
             mem.Write(bts, 0, bts.Length);
             mem.Flush();

             mem.Position = 0;
             byte[] res = new byte[1024];
             mem.Read(res, 0, 1024);

             for (int i = 0; i < res.Length; i++)
             {
                 if(res[i] == (byte)123 || res[i] == (byte)91 || res[i] == (byte)60)//123 = {, 91 = [, 60 = <
                 {
                     tp = (int)res[i];
                     break;
                 }

             }

             mem.Position = 0;


             Console.WriteLine(mem.Length);
             */


            /*
                
            var modelString = "{\"States\": [{\"Name\": \"Texas\",\"Code\": \"TX\"}, {\"Name\": \"New York\",\"Code\": \"NY\"}]}";
            var template = "State Is:{{States[1].Name}}";
            Render(modelString, template);

             modelString = "{\"States\": [{\"Name\": \"Texas\",\"Code\": \"TX\",\"Key\":{\"Value\":\"k1\"}}, {\"Name\": \"New York\",\"Code\": \"NY\",\"Key\":{\"Value\":\"k1\"}}]}";
             template = @" {% for state in States %}
            {{ state.Name }}
            {% endfor %}";
            Render(modelString, template);

            modelString = "{\"States\": [{\"Name\": \"Texas\",\"Code\": \"TX\",\"Key\":{\"Value\":\"k1\"}}, {\"Name\": \"New York\",\"Code\": \"NY\",\"Key\":{\"Value\":\"k2\"}}]}";
            template = "State Is:{{States[1].Key.Value}}";
            Render(modelString, template);

            modelString = "{\"States\": [\"Texas\",\"New York\"]}";
            template = "State Is:{{States[0]}}";
            Render(modelString, template);

            modelString = "{\"States\": [\"Texas\",\"New York\"]}";
            template = "State Is:{{States[0]}}";
            Render(modelString, template);


            modelString = "{\"Name\": \"Emilio\", \"Age\":65}";
            template = "State Is:{{Name}}";
            Render(modelString, template);
            
            modelString = "[\"Agda\", \"Emilio\"]";
            template = "State Is: {{items[0]}}";
            RenderArray(modelString, template);

            string input = "line 1\nline2\nline3";
            string str = checkFroarray(input);
            */
            
           

        }

     
         static void RenderFromXml(string xml, string template)
        {
            var liquidTemplate = DotLiquid.Template.Parse(template);

            var xmlDictionary = XmlToDictionary.Parse(xml);

           
            var result = liquidTemplate.Render(Hash.FromDictionary(xmlDictionary)) ;

/*
            var result = liquidTemplate.Render(Hash.FromAnonymousObject(
  new { data = new XmlNode(modelString) }
));
*/

         Console.WriteLine(result);
        }

      
       
    }
}
