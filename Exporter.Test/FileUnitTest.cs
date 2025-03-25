using Exporter.Test.Source;
using Exporter.Test.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SkillLevelEvaluationExporter.Services;
using SkillLevelEvaluationExporter.Services.Interfaces;

namespace Exporter.Test;

public class FileUnitTest
{
    public FileException exceptions;

    [SetUp]
    public void Setup()
    {
        var fileExceptionPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FileException.json");
        if (File.Exists(fileExceptionPath))
        {
            var json = File.ReadAllText(fileExceptionPath);
            exceptions = JsonConvert.DeserializeObject<FileException>(json) ?? throw new InvalidOperationException();
        }
        else
        {
            exceptions = new FileException
            {
                FileExceptionList = new List<string>(),
                QuestionExceptionDictionary = new Dictionary<string, List<QuestionIdentifier>>()
            };
        }

        Console.WriteLine($"加载例外情况列表：{exceptions.FileExceptionList.Count}个文件，{exceptions.QuestionExceptionDictionary.Count}个题目");
    }
}