using Exporter.Test.Source;
using Exporter.Test.Utils;
using SkillLevelEvaluationExporter.Services;

namespace Exporter.Test.Tests;

[TestFixture]
public class FileExtractorTest:FileUnitTest
{
    [Test,TestCaseSource(typeof(FileSource), nameof(FileSource.GetFiles))]
    public void TestExtract(string filePath)
    {
        var paper = PaperFactory.CreatePaper(filePath);
        Assert.IsNotNull(paper);
    }
}