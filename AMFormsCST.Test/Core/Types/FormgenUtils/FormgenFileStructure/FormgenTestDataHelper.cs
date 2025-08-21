using System.Collections.Generic;
using System.IO;
using System.Xml;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;

public static class FormgenTestDataHelper
{
    public static IEnumerable<object[]> FormgenFilePaths
    {
        get
        {
            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleData", "Formgen Sample Data");
            if (!Directory.Exists(root))
                yield break;

            foreach (var subdir in Directory.GetDirectories(root))
            {
                foreach (var file in Directory.GetFiles(subdir, "*.formgen"))
                {
                    yield return new object[] { file };
                }
            }
        }
    }

    public static DotFormgen LoadDotFormgen(string filePath)
    {
        var doc = new XmlDocument();
        doc.Load(filePath);
        return new DotFormgen(doc.DocumentElement!);
    }
}