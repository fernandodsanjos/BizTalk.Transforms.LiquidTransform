
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace TestLiquid
{
    [Serializable]
    public class ObjectSerialize : IXmlSerializable
    {
        private List<object> objectList;
        public List<object> ObjectList
        {
            set
            {
                objectList = value;
            }
            get
            {
                if (objectList == null)
                    objectList = new List<object>();

                return objectList;
            }
        }

        public XmlSchema GetSchema()
        {
            return new XmlSchema();
        }

        public void ReadXml(XmlReader reader)
        {

        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var obj in ObjectList)
            {
                //Provide elements for object item
                writer.WriteStartElement("Object");
                var properties = obj.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    //Provide elements for per property
                    writer.WriteElementString(propertyInfo.Name, propertyInfo.GetValue(obj).ToString());
                }
                writer.WriteEndElement();
            }
        }
    }
}
