using System.Text;
using SkillLevelEvaluationExporter.Models.Interfaces;
using SkillLevelEvaluationExporter.Properties;

namespace SkillLevelEvaluationExporter.Models;

public class TrueOrFalseQuestion : Question
{
    public override bool IsValid { get; }

    public bool IsCorrect { get; }

    public TrueOrFalseQuestion(
        int majorIndex,
        int minorIndex,
        int buildIndex,
        int questionIndex,
        QuestionLevel questionLevel,
        int pageIndex,
        IList<IContent> content,
        string reference,
        bool isCorrect) : base(majorIndex, minorIndex, buildIndex, questionIndex, pageIndex, QuestionInputType.TrueOrFalse, questionLevel, content, reference)
    {
        IsCorrect = isCorrect;
        IsValid = CheckValid();
    }


    protected bool CheckValid()
    {
        if (base.CheckBasicValid())
        {
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        var baseString = new StringBuilder(base.ToString());
        baseString.Append('\n');
        baseString.AppendLine("A. 正确");
        baseString.AppendLine("B. 错误");
        baseString.AppendLine($"答案: {(IsCorrect ? "A" : "B")}");
        return baseString.ToString();
    }
}