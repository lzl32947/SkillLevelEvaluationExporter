using SkillLevelEvaluationExporter.Models.Interfaces;

namespace SkillLevelEvaluationExporter.Models.Content;

public class ContentText:IContent
{
    public int Length { get; }

    public Guid Guid { get; }

    public string Text { get; }

    public override string ToString()
    {
        return Text;
    }

    public ContentText(string text)
    {
        Text = text;
        Length = text.Length;
        Guid = Guid.NewGuid();
    }
}