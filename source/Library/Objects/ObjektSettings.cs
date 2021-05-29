using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Library.Objects
{
    [XmlRoot("Settings")] 
    public class Settings
    {
        public Settings() {  }

        [XmlElement("Jodkoncentration")]
        public int Jodkoncentration { get; set; }

        [XmlElement("PVK")]
        public string PVK { get; set; }

        [XmlElement("BarnJodDosPerKg")]
        public double BarnJodDosPerKg { get; set; }

        [XmlElement("BarnInjektionstid")]
        public double BarnInjektionstid { get; set; }

        [XmlElement("BarnMaxÅlder")]
        public int BarnMaxÅlder { get; set; }

        [XmlElement("BarnMaxVikt")]
        public int BarnMaxVikt { get; set; }
    }
}
