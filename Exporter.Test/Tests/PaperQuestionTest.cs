using Exporter.Test.Source;
using Exporter.Test.Utils;
using Newtonsoft.Json;
using SkillLevelEvaluationExporter.Models;
using SkillLevelEvaluationExporter.Models.Options;
using SkillLevelEvaluationExporter.Properties;
using SkillLevelEvaluationExporter.Services;

namespace Exporter.Test.Tests;

[TestFixture]
public class PaperQuestionTest : FileUnitTest
{
    [Test, TestCaseSource(typeof(FileSource), nameof(FileSource.GetFiles))]
    public void TestMapFunction(string filePath, string fileName)
    {
        Paper paper = PaperFactory.CreatePaper(filePath,new ExporterOptions{ExportImage = false})!;

        Assert.Multiple(() =>
        {
            var typeMapFile = Path.Combine($"Resources/TypeMap/{fileName}_typeMap.json");
            Assert.IsTrue(File.Exists(typeMapFile), $"文件 {typeMapFile} 不存在");
            var typeMapObj = JsonConvert.DeserializeObject<IList<int[]>>(File.ReadAllText(typeMapFile));
            Assert.IsTrue(TestStructureConverter.IsSameArray(TestStructureConverter.TypeMapToArray(paper.TypeMap), typeMapObj), $"文件 {fileName} 类型映射错误");

            var levelMapFile = Path.Combine($"Resources/LevelMap/{fileName}_levelMap.json");
            Assert.IsTrue(File.Exists(levelMapFile), $"文件 {levelMapFile} 不存在");
            var levelMapObj = JsonConvert.DeserializeObject<IList<int[]>>(File.ReadAllText(levelMapFile));
            Assert.IsTrue(TestStructureConverter.IsSameArray(TestStructureConverter.LevelMapToArray(paper.LevelMap), levelMapObj), $"文件 {fileName} 难度映射错误");

            var pageMapFile = Path.Combine($"Resources/PageMap/{fileName}_pageMap.json");
            Assert.IsTrue(File.Exists(pageMapFile), $"文件 {pageMapFile} 不存在");
            var pageMapObj = JsonConvert.DeserializeObject<IList<int[]>>(File.ReadAllText(pageMapFile));
            Assert.IsTrue(TestStructureConverter.IsSameArray(TestStructureConverter.PageMapToArray(paper.PageMap), pageMapObj), $"文件 {fileName} 页码映射错误");

            var questionIdentifier = TestStructureConverter.QuestionIdentifierToArray(paper.Questions);
            var pageIdentifier = paper.PageMap.Select(x => new int[] { x.Key.Item1, x.Key.Item2, x.Key.Item3 }).ToList();
            Assert.IsTrue(TestStructureConverter.IsSameArray(questionIdentifier, pageIdentifier), $"文件 {fileName} 题号存在错误");
        });
    }
}