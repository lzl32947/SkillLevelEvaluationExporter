using SkillLevelEvaluationExporter.Services;
using SkillLevelEvaluationExporter.Services.Interfaces;

namespace Exporter.Test;

public class Tests
{
    public IList<string> FList = new List<string>();

    [SetUp]
    public void Setup()
    {
        var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Resources"), "*.pdf", SearchOption.AllDirectories);
        FList = files.ToList();
    }

    public void FileTest(string fileNameContains)
    {
        var file = FList.Where(x => x.Contains(fileNameContains)).ToList();
        foreach (var item in file)
        {
            Console.WriteLine($"正在测试:{item}");
            var extractor = new FileExtractor
            {
                FilePath = item,
                WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "Temp"),
                ForceOverride = true
            };
            var paper = extractor.Extract();
            Assert.IsNotNull(paper);
            Console.WriteLine($"测试通过:{item}");
        }
    }

    [Test]
    public void Test信息运维检修工()
    {
        FileTest("信息运维检修工");
        Assert.Pass();
    }

    [Test]
    public void Test土建施工员()
    {
        FileTest("土建施工员");
        Assert.Pass();
    }

    [Test]
    public void Test电网调度自动化厂站端调试检修工()
    {
        FileTest("电网调度自动化厂站端调试检修工");
        Assert.Pass();
    }

    [Test]
    public void Test装表接电工()
    {
        // TODO: 看上去有些计算题没有被正确解析
        FileTest("装表接电工");
        Assert.Pass();
    }
}