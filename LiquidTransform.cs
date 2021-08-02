using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.XLANGs.BaseTypes;
using System.Reflection;
using DotLiquid;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using Microsoft.BizTalk.Streaming;

namespace BizTalk.Transforms.LiquidTransform
{
    public class LiquidTransform : ITransform2
    {
        //https://docs.microsoft.com/en-us/biztalk/core/technical-reference/xslt-custom-transform-implementation
        protected Template Processor;
        public override void Load(string liquid)
        {

            Processor = Template.Parse(liquid);
           
        }

        public override void Transform(Stream input, IDictionary<XmlQualifiedName, object> xsltArguments, Stream results)
        {
            const int jsonObject = 123;
            const int jsonArray = 91;
            const int xmlStart = 60;

            StreamReader inputReader = new StreamReader(input);


            byte[] startBytes = new byte[10];
            input.Read(startBytes, 0, startBytes.Length);

            var fullInput = UTF8Encoding.UTF8.GetString(startBytes) + inputReader.ReadToEnd();

            switch (ObjectType(startBytes))
            {
                case xmlStart:
                    DeserializeXml(fullInput, results);
                    break;
                case jsonArray:
                    DeserializeArray(fullInput, results);
                    break;
                case jsonObject:
                    DeserializeObject(fullInput, results);
                    break;
            }






            //   LocalVariables = Hash.FromAnonymousObject(argument);

        }

        public override void RegisterExtension(string namespaceUri, string assemblyName, string className)
        {
            // Assembly assembly = Assembly.Load(assemblyName);
            // Object obj = assembly.CreateInstance(className);
            //will be used to 
            //- add object to serialize
            //- add liquid filters
            //- add liquid tags



        }

        protected void Render(Hash input, Stream results)
        {
           
            var result = Processor.Render(input);

            using (StreamWriter writer = new StreamWriter(results, Encoding.UTF8, 1024, true))
            {
                writer.Write(result);
                writer.Flush();
            }

            results.Position = 0;

            


        }

        protected int ObjectType(byte[] startBytes)
        {
            int res = 0;
            for (int i = 0; i < startBytes.Length; i++)
            {
                if (startBytes[i] == (byte)123 || startBytes[i] == (byte)91 || startBytes[i] == (byte)60)//123 = {, 91 = [, 60 = <
                {
                    res = (int)startBytes[i];
                    break;
                }

            }

            return res;
        }
        protected void DeserializeXml(string xml, Stream results)
        {
            
            Render(Hash.FromAnonymousObject(
              new { data = new XmlNode(xml) }
            ),results);

            
        }
        protected void DeserializeObject(string json, Stream results)
        {

            dynamic expandoObj = JsonConvert.DeserializeObject<ExpandoObject>(json);

            Render(Hash.FromDictionary(expandoObj),  results);

        }

        protected void DeserializeArray(string json, Stream results)
        {

            dynamic expandoObj = JsonConvert.DeserializeObject<dynamic[]>(json);

            Dictionary<string, object> items = new Dictionary<string, object>();
            items.Add("items", expandoObj);

            Render(Hash.FromDictionary(items),results);

        }

        
    }
}
