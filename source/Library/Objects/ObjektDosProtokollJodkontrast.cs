using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Library.Objects
{
    [XmlRoot("ProtokollJodkontrast")]
    public class ProtokollJodkontrast
    {
        public ProtokollJodkontrast() { }
        
        [XmlElement("ID")]
        public int ID { get; set; }

        [XmlElement("ProtokollNamn")]
        public string ProtokollNamn { get; set; }

        [XmlElement("DosPerKg")]
        public int DosPerKg { get; set; }

        [XmlElement("JodKoncentration")]
        public int JodKoncentration { get; set; }

        [XmlElement("Injektionstid")]
        public double Injektionstid { get; set; }

        [XmlElement("MaxVikt")]
        public int MaxVikt { get; set; }

        [XmlElement("Anteckningar")]
        public string Anteckningar { get; set; }
    }
}
