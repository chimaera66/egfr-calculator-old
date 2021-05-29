using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Library.Objects
{
    [XmlRoot("GdKontrast")]
    public class GdKontrast
    {
        public GdKontrast() { }

        [XmlElement("ID")]
        public int ID { get; set; }

        [XmlElement("GdNamn")]
        public string GdNamn { get; set; }

        [XmlElement("DosPerKg")]
        public double DosPerKg { get; set; }

        //[XmlElement("GdKoncentration")]
        //public string GdKoncentration { get; set; }

        [XmlElement("GdRiskNivå")]
        public int GdRiskNivå { get; set; }

        [XmlElement("Förpackningsmängd")]
        private List<GdFörpackning> GDFörpackning = new List<GdFörpackning>();
        public List<GdFörpackning> gdförpackning
        {
            get { return GDFörpackning; }
            set { GDFörpackning = value; }
        }

        [XmlElement("Anteckningar")]
        public string Anteckningar { get; set; }
    }

    public class GdFörpackning
    {
        public GdFörpackning() { }

        [XmlElement("förpackningsID")]
        public int förpackningsID { get; set; }
        [XmlElement("mängd")]
        public int mängd { get; set; }
        [XmlElement("lägstados")]
        public double lägstados { get; set; }
        [XmlElement("högstados")]
        public double högstados { get; set; }
    }

}
