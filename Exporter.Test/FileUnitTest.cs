using Exporter.Test.Source;
using Exporter.Test.Utils;
using Newtonsoft.Json;
using SkillLevelEvaluationExporter.Services;
using SkillLevelEvaluationExporter.Services.Interfaces;

namespace Exporter.Test;

public class FileUnitTest
{

    [SetUp]
    public void Setup()
    {
        if (false)
        {
            ExportTestResources();
        }
    }

    public static void ExportTestResources()
    {
        var files = FileSource.GetFiles();
        foreach (var data in files)
        {
            var filePath = data[0];
            var fileName = data[1];

            try
            {
                var paper = PaperFactory.CreatePaper(filePath);
                if (paper is null)
                {
                    Console.WriteLine($"{fileName} 为 null, 跳过");
                }

                if (!Directory.Exists("Data"))
                {
                    Directory.CreateDirectory("Data");
                }

                var levelMap = TestStructureConverter.LevelMapToArray(paper.LevelMap);
                if (!Directory.Exists("Data/LevelMap"))
                {
                    Directory.CreateDirectory("Data/LevelMap");
                }
                var levelMapObj = JsonConvert.SerializeObject(levelMap, Formatting.Indented);
                File.WriteAllText($"Data/LevelMap/{fileName}_levelMap.json", levelMapObj);

                var typeMap = TestStructureConverter.TypeMapToArray(paper.TypeMap);
                if (!Directory.Exists("Data/TypeMap"))
                {
                    Directory.CreateDirectory("Data/TypeMap");
                }
                var typeMapObj = JsonConvert.SerializeObject(typeMap, Formatting.Indented);
                File.WriteAllText($"Data/TypeMap/{fileName}_typeMap.json", typeMapObj);

                var pageMap =  TestStructureConverter.PageMapToArray(paper.PageMap);
                if (!Directory.Exists("Data/PageMap"))
                {
                    Directory.CreateDirectory("Data/PageMap");
                }
                var pageMapObj = JsonConvert.SerializeObject(pageMap, Formatting.Indented);
                File.WriteAllText($"Data/PageMap/{fileName}_pageMap.json", pageMapObj);

                var questionIdentifier = TestStructureConverter.QuestionIdentifierToArray(paper.Questions);
                if (!Directory.Exists("Data/QuestionIdentifier"))
                {
                    Directory.CreateDirectory("Data/QuestionIdentifier");
                }
                var questionIdentifierObj = JsonConvert.SerializeObject(questionIdentifier, Formatting.Indented);
                File.WriteAllText($"Data/QuestionIdentifier/{fileName}_questionIdentifier.json", questionIdentifierObj);
            }catch (Exception e)
            {
                Console.WriteLine($"{fileName} 出现错误: {e.Message}");
            }
        }
    }
}