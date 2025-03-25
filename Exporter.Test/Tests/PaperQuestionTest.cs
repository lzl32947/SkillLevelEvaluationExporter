using Exporter.Test.Source;
using Exporter.Test.Utils;
using Newtonsoft.Json;
using SkillLevelEvaluationExporter.Models;
using SkillLevelEvaluationExporter.Models.Options;
using SkillLevelEvaluationExporter.Properties;
using SkillLevelEvaluationExporter.Services;
using SkillLevelEvaluationExporter.Utils;

namespace Exporter.Test.Tests;

[TestFixture]
public class PaperQuestionTest : FileUnitTest
{
    [Test, TestCaseSource(typeof(FileSource), nameof(FileSource.GetFiles))]
    public void TestMapFunction(string filePath)
    {
        Paper paper = PaperFactory.CreatePaper(filePath,new ExporterOptions{ExportImage = false})!;
        var fileName = Path.GetFileNameWithoutExtension(filePath);

        var questionIdentifier = StructureConverter.QuestionIdentifierToArray(paper.Questions);
        var pageIdentifier = paper.PageMap.Select(x => new int[] { x.Key.Item1, x.Key.Item2, x.Key.Item3 }).ToList();
        Assert.IsTrue(StructureConverter.IsSameArray(questionIdentifier, pageIdentifier), $"文件 {fileName} 题号存在错误");
        var sequence = QuestionUtil.FindNonContinuousSequences(paper.Questions.Select(x => (x.MajorIndex, x.MinorIndex, x.BuildIndex)).ToList());
        Assert.IsTrue(sequence.Count == 0, $"文件 {fileName} 题号不连续, 缺失题号: {string.Join(",", sequence.Select(x => QuestionUtil.Index2JointString([x.Item1, x.Item2, x.Item3])))}");
    }
}