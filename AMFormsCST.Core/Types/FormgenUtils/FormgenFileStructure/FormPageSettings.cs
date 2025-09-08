using AMFormsCST.Core.Attributes;
using AMFormsCST.Core.Interfaces.Attributes;
using System.Xml;

namespace AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure
{
    public partial class FormPageSettings : IEquatable<FormPageSettings>, INotifyPropertyChanged
    {
        [NotifyPropertyChanged]
        private int _pageNumber;
        [NotifyPropertyChanged]
        private int _defaultFontSize;
        [NotifyPropertyChanged]
        private int _leftPrinterMargin;
        [NotifyPropertyChanged]
        private int _rightPrinterMargin;
        [NotifyPropertyChanged]
        private int _topPrinterMargin;
        [NotifyPropertyChanged]
        private int _bottomPrinterMargin;

        public FormPageSettings() { }

        public FormPageSettings(XmlAttributeCollection attributes)
        {
            if (int.TryParse(attributes[0].Value, out int parsedInt))
                PageNumber = parsedInt;

            if (int.TryParse(attributes[1].Value, out parsedInt))
                DefaultFontSize = parsedInt;

            if (int.TryParse(attributes[2].Value, out parsedInt))
                LeftPrinterMargin = parsedInt;

            if (int.TryParse(attributes[3].Value, out parsedInt))
                RightPrinterMargin = parsedInt;

            if (int.TryParse(attributes[4].Value, out parsedInt))
                TopPrinterMargin = parsedInt;

            if (int.TryParse(attributes[5].Value, out parsedInt))
                BottomPrinterMargin = parsedInt;
        }

        public void GenerateXml(XmlWriter xml)
        {
            xml.WriteAttributeString("pageNumber", PageNumber.ToString());
            xml.WriteAttributeString("defaultPoints", DefaultFontSize.ToString());
            xml.WriteAttributeString("leftPrinterMargin", LeftPrinterMargin.ToString());
            xml.WriteAttributeString("rightPrinterMargin", RightPrinterMargin.ToString());
            xml.WriteAttributeString("topPrinterMargin", TopPrinterMargin.ToString());
            xml.WriteAttributeString("bottomPrinterMargin", BottomPrinterMargin.ToString());

        }

        public bool Equals(FormPageSettings? other) =>
            other is not null &&
            PageNumber == other.PageNumber &&
            DefaultFontSize == other.DefaultFontSize &&
            LeftPrinterMargin == other.LeftPrinterMargin &&
            RightPrinterMargin == other.RightPrinterMargin &&
            TopPrinterMargin == other.TopPrinterMargin &&
            BottomPrinterMargin == other.BottomPrinterMargin;

        public override bool Equals(object? obj) => Equals(obj as FormPageSettings);
        public override int GetHashCode() => HashCode.Combine(PageNumber, DefaultFontSize, 
            LeftPrinterMargin, RightPrinterMargin, TopPrinterMargin, BottomPrinterMargin);
    }
}
