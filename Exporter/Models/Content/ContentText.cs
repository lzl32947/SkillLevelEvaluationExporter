using SkillLevelEvaluationExporter.Interfaces;

namespace SkillLevelEvaluationExporter.Models.Content;

public class ContentText:IContent
{
    public int Length { get; }

    public Guid Guid { get; }

    public string Text { get; }

    public ContentText(string text)
    {
        Text = text;
        Length = text.Length;
        Guid = Guid.NewGuid();
    }
}