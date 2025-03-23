using SkillLevelEvaluationExporter.Models;
using SkillLevelEvaluationExporter.Services;

namespace Exporter.Test.Utils;

public static class PaperFactory
{
    public static Paper? CreatePaper(string filePath)
    {
        var extractor = new FileExtractor
        {
            FilePath = filePath,
            WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "Temp"),
            ForceOverride = true
        };
        var paper = extractor.Extract();
        return paper;
    }
}