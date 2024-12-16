using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApplication1.Serializers
{
    class Serializers<T> // where T : new()
    {
        public void Test(T t)
        {
            // Read and write purchase orders.  
            Write(t, "po.xml");
            var t2 = Read("po.xml");
        }

        private void Write(T t, string filename)
        {
            // Creates an instance of the XmlSerializer class;  
            // specifies the type of object to serialize.  
            XmlSerializer serializer =
            new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(filename))
            {
                // Serializes the purchase order, and closes the TextWriter.  
                serializer.Serialize(writer, t);
                writer.Close();
            }
        }

        protected T Read(string filename)
        {
            // Creates an instance of the XmlSerializer class;  
            // specifies the type of object to be deserialized.  
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            // If the XML document has been altered with unknown   
            // nodes or attributes, handles them with the   
            // UnknownNode and UnknownAttribute events.  
            serializer.UnknownNode += serializer_UnknownNode;
            serializer.UnknownAttribute += serializer_UnknownAttribute;

            // A FileStream is needed to read the XML document.  
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                return (T)serializer.Deserialize(fs);
            }
        }

        protected void serializer_UnknownNode
        (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        protected void serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}
