using System;
using System.Xml;

namespace AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure
{
    public class DotFormgenSettings
    {
        public int Version { get; set; }
        public string UUID { get; set; } = string.Empty;
        public bool LegacyImport { get; set; }
        public int TotalPages { get; set; }
        public int DefaultFontSize { get; set; }
        public bool MissingSourceJpeg { get; set; }
        public bool Duplex { get; set; }
        public int MaxAccessoryLines { get; set; }
        public bool PreprintedLaserForm { get; set; }

        public DotFormgenSettings() { }

        public DotFormgenSettings(XmlAttributeCollection attributes)
        {
            if (int.TryParse(attributes[0].Value, out int parsedInt))
                Version = parsedInt;

            UUID = attributes[1].Value;

            if (bool.TryParse(attributes[2].Value, out bool parsedBool))
                LegacyImport = parsedBool;

            if (int.TryParse(attributes[3].Value, out parsedInt))
                TotalPages = parsedInt;

            if (int.TryParse(attributes[4].Value, out parsedInt))
                DefaultFontSize = parsedInt;

            if (bool.TryParse(attributes[5].Value, out parsedBool))
                MissingSourceJpeg = parsedBool;

            if (bool.TryParse(attributes[6].Value, out parsedBool))
                Duplex = parsedBool;

            if (int.TryParse(attributes[7].Value, out parsedInt))
                MaxAccessoryLines = parsedInt;

            if (bool.TryParse(attributes[8].Value, out parsedBool))
                PreprintedLaserForm = parsedBool;
        }
        public void GenerateXML(XmlWriter xml)
        {
            xml.WriteAttributeString("version", Version.ToString());
            xml.WriteAttributeString("publishedUUID", UUID);
            xml.WriteAttributeString("legacyImport", LegacyImport.ToString().ToLowerInvariant());
            xml.WriteAttributeString("totalPages", TotalPages.ToString());
            xml.WriteAttributeString("defaultPoints", DefaultFontSize.ToString());
            xml.WriteAttributeString("missingSourceJpeg", MissingSourceJpeg.ToString().ToLowerInvariant());
            xml.WriteAttributeString("duplex", Duplex.ToString().ToLowerInvariant());
            xml.WriteAttributeString("maxAccessoryLines", MaxAccessoryLines.ToString());
            xml.WriteAttributeString("prePrintedLaserForm", PreprintedLaserForm.ToString().ToLowerInvariant());
        }
    }

}
