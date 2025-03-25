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
    public void TestQuestion(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Assert.Ignore($"文件 {filePath} 不存在, 跳过测试");
        }

        var md5 = FileUtil.CalculateMD5(filePath);
        if (exceptions.FileExceptionList.Contains(md5))
        {
            Assert.Ignore($"文件 {filePath} 存在异常, 跳过测试");
        }

        var paper = PaperFactory.CreatePaper(filePath, new ExporterOptions { ExportImage = false })!;

        var fileName = Path.GetFileNameWithoutExtension(filePath);

        var questionIdentifier = StructureConverter.ParseQuestionIdentifier(paper.Questions);
        var pageIdentifier = paper.PageMap.Select(x => new QuestionIdentifier
        {
            Major = x.Key.Item1,
            Minor = x.Key.Item2,
            Build = x.Key.Item3
        }
    ).ToList();
        var differences = StructureConverter.DifferenceQuestions(questionIdentifier, pageIdentifier);
        var inAbutNotB = differences.Item1;
        var inBButNotA = differences.Item2;
        exceptions.QuestionExceptionDictionary.TryGetValue(md5, out var questionExceptionList);
        if (questionExceptionList != null)
        {
            inAbutNotB = inAbutNotB.Where(x =>
            {
                if (questionExceptionList.Any(y => y.Major == x.Major && y.Minor == x.Minor && y.Build == x.Build))
                {
                    return false;
                }

                return true;
            }).ToList();
        }
        if (questionExceptionList != null)
        {
            inBButNotA = inBButNotA.Where(x =>
            {
                if (questionExceptionList.Any(y => y.Major == x.Major && y.Minor == x.Minor && y.Build == x.Build))
                {
                    return false;
                }

                return true;
            }).ToList();
        }
        Assert.IsTrue(inAbutNotB.Count == 0 && inBButNotA.Count == 0, $"文件 {fileName} 题号不匹配, 多余题号: {string.Join(",", inAbutNotB.Select(x => x.ToString()))}, 存在缺失题号: {string.Join(",", inBButNotA.Select(x => x.ToString()))}");
        var items = paper.Questions.Select(x => new QuestionIdentifier
        {
            Major = x.MajorIndex,
            Minor = x.MinorIndex,
            Build = x.BuildIndex
        }).ToList();
        if (questionExceptionList != null)
        {
            items.AddRange(questionExceptionList);
        }
        var sequence = QuestionTestUtil.FindNonContinuousSequences(items);


        Assert.IsTrue(sequence.Count == 0, $"文件 {fileName} 题号不连续, 缺失题号: {string.Join(",",  sequence.Select(x => x.ToString()))}");
    }
}