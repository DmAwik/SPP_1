using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace SPP1.Serialize
{
    class MyXmlSerializer : ISerializer
    {
        private XmlSerializerNamespaces emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        public string Serialize(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(stringWriter, obj, emptyNamespaces);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
