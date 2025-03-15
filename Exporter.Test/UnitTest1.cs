using SkillLevelEvaluationExporter.Services;
using SkillLevelEvaluationExporter.Services.Interfaces;

namespace Exporter.Test;


public class Tests
{

    public IList<string> FList = new List<string>();

    [SetUp]
    public void Setup()
    {
        var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory,"Resources"), "*.pdf", SearchOption.AllDirectories);
        FList = files.ToList();
    }

    [Test]
    public void Test1()
    {
        foreach (var file in FList)
        {
            var extractor = new FileExtractor();
            extractor.FilePath = file;
            extractor.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "Temp");
            var paper = extractor.Extract();
            Assert.IsNotNull(paper);
        }
        Assert.Pass();
    }
}