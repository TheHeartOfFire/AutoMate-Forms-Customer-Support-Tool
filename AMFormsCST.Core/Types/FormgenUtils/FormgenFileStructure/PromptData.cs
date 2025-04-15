using System.Collections.Generic;
using System.Xml;

namespace AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure
{
    public class PromptData
    {
        public PromptDataSettings? Settings { get; set; }
        public string? Message { get; set; }
        public List<string> Choices { get; set; } = [];
        public PromptData(XmlNode node)
        {
            if (node.Attributes != null) Settings = new PromptDataSettings(node.Attributes);

            if (node.FirstChild is { HasChildNodes: true })
                Message = node.FirstChild.FirstChild?.InnerText;

            foreach (XmlNode child in node.ChildNodes)
                if (child.Name == "choices")
                    Choices.Add(child.InnerText);
        }

        internal void GenerateXml(XmlWriter xml)
        {
            xml.WriteStartElement("promptData");
            Settings?.GenerateXml(xml);

            xml.WriteStartElement("promptMessage");
            xml.WriteString(Message);
            xml.WriteEndElement();

            foreach (var choice in Choices)
            {
                xml.WriteStartElement("choices");
                xml.WriteString(choice);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();

        }
    }
}
