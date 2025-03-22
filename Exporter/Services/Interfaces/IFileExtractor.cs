using SkillLevelEvaluationExporter.Models;

namespace SkillLevelEvaluationExporter.Services.Interfaces;

public interface IFileExtractor
{
    public Paper? Extract();
}