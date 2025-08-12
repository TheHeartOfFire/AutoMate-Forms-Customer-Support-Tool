using System.Collections.Generic;
using System.Xml;

namespace AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure
{
    public class FormPage
    {
        public FormPageSettings? Settings { get; set; }
        public List<FormField> Fields { get; set; } = new();

        public FormPage()
        {
            Settings = new FormPageSettings();
        }

        public FormPage(XmlNode node)
        {
            if (node.Attributes != null) Settings = new FormPageSettings(node.Attributes);

            if (node.FirstChild == null) return;
            foreach (XmlNode child in node.FirstChild)
            {
                Fields.Add(new FormField(child));
            }
        }

        public void GenerateXml(XmlWriter xml)
        {

            xml.WriteStartElement("pages");
            Settings?.GenerateXml(xml);

            xml.WriteStartElement("fields");
            foreach (var field in Fields)
            {
                xml.WriteStartElement("entry");
                field.GenerateXml(xml);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
            xml.WriteEndElement();
        }
    }
}
