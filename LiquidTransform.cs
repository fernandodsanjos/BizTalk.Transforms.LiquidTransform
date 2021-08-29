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
using System.Runtime.InteropServices;
using System.Globalization;

namespace BizTalk.Transforms.LiquidTransform
{
    public class LiquidTransform : ITransform2
    {
        //https://docs.microsoft.com/en-us/biztalk/core/technical-reference/xslt-custom-transform-implementation
        protected Template Processor;
        protected RenderParameters parameters;
        protected Hash variables;

        protected RenderParameters Parameters
        {
            get
            {
                if (parameters == null)
                    parameters = new RenderParameters(System.Globalization.CultureInfo.CurrentCulture);

                return parameters;
            }
        }



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

            input.Position = 0;

           

            switch (ObjectType(startBytes))
            {
                case xmlStart:
                    DeserializeXml(input, results);
                    break;
                case jsonArray:
                    DeserializeArray(input, results);
                    break;
                case jsonObject:
                    DeserializeObject(input, results);
                    break;
            }


            

            //   LocalVariables = Hash.FromAnonymousObject(argument);

        }

        public override void RegisterExtension(string namespaceUri, string assemblyName, string className)
        {
            if (namespaceUri == "BizTalk.Transforms.LiquidTransform.ILiquidRegister")
            {
                dynamic extension = null;

                try
                {
                    Assembly assembly = Assembly.Load(assemblyName);

                    extension = assembly.CreateInstance(className);
                }
                catch (Exception)
                {

                    throw new Exception($"RegisterExtension: Could not load  {assemblyName} {className}");
                }

                extension.Register(Parameters);
                

            }



        }

        protected void Render(dynamic input, Stream results)
        {
            if(Parameters.LocalVariables == null)
            {
                Parameters.LocalVariables = Hash.FromDictionary(input);
            }
            else
            {
                Hash hash = Parameters.LocalVariables;
                hash.Merge(input);
                
            }
            
            Processor.Render(results, Parameters);
           
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
        protected void DeserializeXml(Stream xml, Stream results)
        {
            var xmlDictionary = XmlToDictionary.Parse(xml);

            Render(xmlDictionary, results);


        }
        protected void DeserializeObject(Stream json, Stream results)
        {
            StreamReader reader = new StreamReader(json);
            dynamic expandoObj = JsonConvert.DeserializeObject<ExpandoObject>(reader.ReadToEnd());

            Render(expandoObj,  results);

        }

        protected void DeserializeArray(Stream json, Stream results)
        {
            StreamReader reader = new StreamReader(json);
            dynamic expandoObj = JsonConvert.DeserializeObject<dynamic[]>(reader.ReadToEnd());

            Dictionary<string, object> items = new Dictionary<string, object>();
            items.Add("items", expandoObj);

            Render(items,results);

        }

        
    }
}
