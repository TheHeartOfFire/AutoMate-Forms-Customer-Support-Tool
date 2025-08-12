using System.Xml;

namespace AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure
{
    public class FormField
    {
        public FormFieldSettings? Settings { get; set; }
        public string? Expression { get; set; }
        public string? SampleData { get; set; }
        public FormatOption FormattingOption { get; set; }

        public enum FormatOption
        {
            None,
            EmptyZeroPrintsNothing,
            EmptyFieldPrints0,
            NumbersAsWords,
            NAIfBlank
        }

        public FormField()
        {
            Settings = new FormFieldSettings();
        }

        public FormField(XmlNode node)
        {
            var xmlAttributeCollection = node.ChildNodes[1]?.Attributes;
            if (xmlAttributeCollection != null)
                Settings = new FormFieldSettings(xmlAttributeCollection);


            var innerText = node.ChildNodes[1]?.ChildNodes[0]?.InnerText;
            if (innerText != null)
                Expression = innerText;

            var sampleData = node.ChildNodes[1]?.ChildNodes[1]?.InnerText;
            if (sampleData != null)
                SampleData = sampleData;

            var option = node.ChildNodes[1]?.ChildNodes[2]?.InnerText;
            if (option != null)
                FormattingOption = GetFormatOption(option);
        }
        public static FormatOption GetFormatOption(string option) => option switch
        {
            "None" => FormatOption.None,
            "EmptyZeroPrintsNothing" => FormatOption.EmptyZeroPrintsNothing,
            "BlankPrintsZero" => FormatOption.EmptyFieldPrints0,
            "NumberAsWords" => FormatOption.NumbersAsWords,
            "NAIfBlank" => FormatOption.NAIfBlank,
            _ => FormatOption.None,
        };

        public static string GetFormatOption(FormatOption option) => option switch
        {
            FormatOption.None => "None",
            FormatOption.EmptyZeroPrintsNothing => "EmptyZeroPrintsNothing",
            FormatOption.EmptyFieldPrints0 => "BlankPrintsZero",
            FormatOption.NumbersAsWords => "NumberAsWords",
            FormatOption.NAIfBlank => "NAIfBlank",
            _ => "None",
        };

        public void GenerateXml(XmlWriter xml)
        {
            xml.WriteStartElement("key");
            xml.WriteString(Settings?.ID.ToString());
            xml.WriteEndElement();

            xml.WriteStartElement("value");
            Settings?.GenerateXml(xml);

            xml.WriteStartElement("expression");
            xml.WriteString(Expression);
            xml.WriteEndElement();

            xml.WriteStartElement("sampleData");
            xml.WriteString(SampleData);
            xml.WriteEndElement();

            xml.WriteStartElement("formatOption");
            xml.WriteString(GetFormatOption(FormattingOption));
            xml.WriteEndElement();

            xml.WriteEndElement();

        }
    }
}
