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
namespace TestLiquid
{
    class Program
    {
        static void Main(string[] args)
        {

            
           var template = File.ReadAllText(@"C:\repos\LiquidTransform\BtsTestLiquid\Map1.liquid");

           BizTalk.Transforms.LiquidTransform.LiquidTransform liquid = new BizTalk.Transforms.LiquidTransform.LiquidTransform();

           liquid.Load(template);

           FileStream input = new FileStream(@"C:\repos\LiquidTransform\Testfiles\person.json", FileMode.Open);
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
            mem.Write(bts,0,bts.Length); 
            mem.Position = 0;


            output = new MemoryStream();
            liquid.Transform(mem, null, output);

            outReder = new StreamReader(output);
            Console.WriteLine(outReder.ReadToEnd());


            template = File.ReadAllText(@"C:\repos\LiquidTransform\BtsTestLiquid\xmltojson.liquid");

            liquid = new BizTalk.Transforms.LiquidTransform.LiquidTransform();

            liquid.Load(template);

             input = new FileStream(@"C:\repos\LiquidTransform\BtsTestLiquid\persons.xml", FileMode.Open);
             output = new MemoryStream();
            liquid.Transform(input, null, output);

             outReder = new StreamReader(output);
            Console.WriteLine(outReder.ReadToEnd());


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
            /*
            string modelString = @"<persons><person>
  <name>
    <first>Deane</first>
    <last>Barker</last>
  </name>
</person>
<person>
  <name>
    <first>Fernnado</first>
    <last>Barker</last>
  </name>
</person>
</persons>";

           
           
             string template = "Order Id:{{data.list['//person'][1].name.first}}";
            RenderFromXml(modelString, template);

             template = "Order Id:{{data.list-person[1].name.first}}";
            RenderFromXml(modelString, template);
*/
            Console.ReadKey();
        }

        static void CheckContent(Stream stm)
        {
            byte[] bt = new byte[100];

            stm.Read(bt, 0, bt.Length);

            for (int i = 0; i < bt.Length; i++)
            {
               
            }

            stm.Position = 0;


        }
         static void RenderFromXml(string modelString, string template)
        {
            var liquidTemplate = DotLiquid.Template.Parse(template);

            



            var result = liquidTemplate.Render(Hash.FromAnonymousObject(
  new { data = new XmlNode(modelString) }
));

            Console.WriteLine(result);
        }

        static void Render(string json, string template)
        {
           
            dynamic expandoObj = JsonConvert.DeserializeObject<ExpandoObject>(json);

            var liquidTemplate = DotLiquid.Template.Parse(template);

            Hash hash = Hash.FromDictionary(expandoObj);
            var result = liquidTemplate.Render(hash);

            Console.WriteLine(result);
        }

        static void RenderArray(string json, string template)
        {
            
            dynamic expandoObj = JsonConvert.DeserializeObject<dynamic[]>(json);

            Dictionary<string, object> items = new Dictionary<string, object>();
            items.Add("items", expandoObj);
  
           
            var liquidTemplate = DotLiquid.Template.Parse(template);
            var result = liquidTemplate.Render(Hash.FromDictionary(items));

            Console.WriteLine(result);
        }

        static string checkFroarray(string input)
        {
            StringReader inputReader = new StringReader(input);

            var firstline = inputReader.ReadLine().TrimStart();

            var fullInput = firstline + inputReader.ReadToEnd();

            return fullInput;
        }
       
    }
}
