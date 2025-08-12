using System.Xml;

namespace AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure
{
    public class CodeLine
    {
        public CodeLineSettings? Settings { get; set; }
        public string? Expression { get; set; }
        public PromptData? PromptData { get; set; }
        public CodeLine(XmlNode node)
        {
            if (node.Attributes != null) Settings = new CodeLineSettings(node.Attributes);

            if (Settings != null && Settings.Type != CodeLineSettings.CodeType.PROMPT)
            {
                if (node.FirstChild != null) Expression = node.FirstChild.InnerText;
            }
            else if (node.FirstChild != null) PromptData = new PromptData(node.FirstChild);
        }
        public CodeLine(CodeLine codeLine, string? newName, int newIndex)
        {
            if (codeLine.Settings != null) Settings = new CodeLineSettings(codeLine.Settings, newName, newIndex);
            Expression = codeLine.Expression;
            PromptData = codeLine.PromptData;
        }
        public CodeLine()
        {
            Settings = new CodeLineSettings();
            PromptData = new PromptData();
        }

        public void GenerateXml(XmlWriter xml)
        {
            xml.WriteStartElement("codeLines");
            if (Settings == null) return;
            Settings.GenerateXml(xml);

            if (Settings.Type != CodeLineSettings.CodeType.PROMPT)
            {
                xml.WriteStartElement("expression");
                xml.WriteString(Expression);
                xml.WriteEndElement();
            }
            else
            {
                var promptData = PromptData;
                promptData?.GenerateXml(xml);
            }

            xml.WriteEndElement();
        }
    }
}
