using SkillLevelEvaluationExporter.Models;
using SkillLevelEvaluationExporter.Models.Options;
using SkillLevelEvaluationExporter.Services;

namespace Exporter.Test.Utils;

public static class PaperFactory
{
    public static Paper? CreatePaper(string filePath, ExporterOptions? options = null)
    {
        var extractor = new FileExtractor
        {
            FilePath = filePath,
            WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "Temp"),
            ForceOverride = true,
            Options = options ?? new ExporterOptions()
        };
        var paper = extractor.Extract();
        return paper;
    }
}