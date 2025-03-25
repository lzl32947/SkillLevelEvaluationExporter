using System.Security.Cryptography;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
using SkillLevelEvaluationExporter.Utils;

namespace SkillLevelEvaluationExporter.Models.Content;

public class ContentText
{
    public string Text { get; }

    public string Md5 { get; }

    public override string ToString()
    {
        return Text;
    }

    public ContentText(string text)
    {
        Text = text;
        Md5 = FileUtil.CalculateStringMd5(text);
    }

    public override bool Equals(object? obj)
    {
        return obj is ContentText text &&
               Text == text.Text;
    }

    public override int GetHashCode()
    {
        return Md5.GetHashCode();
    }
}