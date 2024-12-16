using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Model
{
    public class Chest : IXmlSerializable
    {
        [Key]
        public int Id { get; set; }

        public double Food { get; set; }
        public double Straw { get; set; }

        public double Wood { get; set; }
        public double Stone { get; set; }
        public double Iron { get; set; }

        public double Gold { get; set; }

        public double Tools { get; set; }
        public double Silk { get; set; }
        public double Glasware { get; set; }
        public double Marble { get; set; }
        public double Gem { get; set; }
        public double Weapons { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadToDescendant("Yemek");
            Food = reader.ReadElementContentAsDouble();
            reader.ReadToDescendant("Odun");
            Wood = reader.ReadElementContentAsDouble();
            reader.ReadToDescendant("Tas");
            Stone = reader.ReadElementContentAsDouble();
        }

        public void WriteXml(XmlWriter writer)
        {
            //writer.WriteStartElement();
            writer.WriteElementString("Yemek", Food.ToString("0.######"));
            writer.WriteElementString("Odun", Wood.ToString("0.######"));
            writer.WriteElementString("Tas", Stone.ToString("0.######"));
            //writer.WriteEndElement();
        }
    }
}
