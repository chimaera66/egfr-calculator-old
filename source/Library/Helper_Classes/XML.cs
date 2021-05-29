using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//XML
using System.Xml;
using System.Xml.Serialization;
using System.IO;
//Project Classes
using Library.Objects;

namespace Library.Helper_Classes
{
    public class XML
    {
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //SERIALIZE


        public static string path;

        //GETPATH
        #region Hämtar sökvägen till exefilen
        public static void GetPath()
        {
            //hämtar dir från vilken exe filen körs
            path = System.IO.Path.GetDirectoryName(
               System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);
            path += "\\";
            //MessageBox.Show(path);
        }
        #endregion

        //Laddar/sparar från/till XML
        #region XMLSerialiserarna
        //static List<GdKontrast> DeserializeFromXMLGdKontrast(string filename)
        //{
        //    filename = Variables.path + filename;
        //    XmlSerializer deserializer = new XmlSerializer(typeof(List<GdKontrast>));
        //    TextReader textReader = new StreamReader(@filename);
        //    List<GdKontrast> list;
        //    list = (List<GdKontrast>)deserializer.Deserialize(textReader);
        //    textReader.Close();
        //    return list;
        //}

        public static List<Settings> DeserializeFromXMLSettings(string filename)
        {
            GetPath();
            filename = path + filename;
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Settings>));
            TextReader textReader = new StreamReader(@filename);
            List<Settings> list;
            list = (List<Settings>)deserializer.Deserialize(textReader);
            textReader.Close();
            return list;
        }

        public static List<PVK> DeserializeFromXMLPVK(string filename)
        {
            GetPath();
            filename = path + filename;
            XmlSerializer deserializer = new XmlSerializer(typeof(List<PVK>));
            TextReader textReader = new StreamReader(@filename);
            List<PVK> list;
            list = (List<PVK>)deserializer.Deserialize(textReader);
            textReader.Close();
            return list;
        }

        public static List<ProtokollJodkontrast> DeserializeFromXMLDosProtokoll(string filename)
        {
            GetPath();
            filename = path + filename;
            XmlSerializer deserializer = new XmlSerializer(typeof(List<ProtokollJodkontrast>));
            TextReader textReader = new StreamReader(@filename);
            List<ProtokollJodkontrast> list;
            list = (List<ProtokollJodkontrast>)deserializer.Deserialize(textReader);
            textReader.Close();
            return list;
        }
        #endregion

        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
    }
}
