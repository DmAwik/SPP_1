using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Tracer
{
    [XmlRoot("root")]
    public class TraceResult
    {
        [XmlElement(ElementName = "thread")]
        public List<Threads> Threads = new List<Threads>();
    }
    public class Method
    {
        [XmlAttribute("time")]
        public long Time;
        [XmlElement(ElementName = "method")]
        public List<Methods> Methods;
    }
    public class Threads : Method
    {
        [XmlAttribute("id")]
        public int id;
    }
    public class Methods : Method
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("class")]
        public string Class;
    }
}
