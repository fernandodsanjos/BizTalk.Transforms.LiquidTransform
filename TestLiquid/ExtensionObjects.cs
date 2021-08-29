using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace TestLiquid
{
    [XmlRoot("ExtensionObjects")]
    public class ExtensionObjects
    {
       [XmlElement("ExtensionObject")]
        public List<ExtensionObject> Extensions { get; set; }
    }

    
    public class ExtensionObject
    {
        [XmlAttribute]
        public string Namespace { get; set; }
        [XmlAttribute]
        public string AssemblyName { get; set; }
        [XmlAttribute]
        public string ClassName { get; set; }
    }
}
