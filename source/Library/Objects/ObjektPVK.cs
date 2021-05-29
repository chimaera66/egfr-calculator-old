using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Library.Objects
{
    [XmlRoot("PVK")] 
    public class PVK
    {
        public PVK() {  }

        [XmlElement("PVKID")]
        public int PVKID { get; set; }

        [XmlElement("namn")]
        public string namn { get; set; }

        [XmlElement("storlek")]
        private List<PVKstorlek> Storlek = new List<PVKstorlek>();
        public List<PVKstorlek> storlek
        {
            get { return Storlek; }
            set { Storlek = value; }
        }
    }

    public class PVKstorlek
    {
        public PVKstorlek() { }

        [XmlElement("storleksID")]
        public int storleksID { get; set; }
        [XmlElement("färg")]
        public string färg { get; set; }
        [XmlElement("g")]
        public int g { get; set; }
        [XmlElement("ytterdiameter")]
        public double ytterdiameter { get; set; }
        [XmlElement("maxflöde")]
        public double maxflöde { get; set; }
    }
}
