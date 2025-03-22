using System.Text;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
using SkillLevelEvaluationExporter.Properties;

namespace SkillLevelEvaluationExporter.Models.Questions;

public class CalculationQuestion : Question
{
    public string Answer { get; }

    public IList<IContent> Solve { get; }

    public CalculationQuestion(
        int majorIndex,
        int minorIndex,
        int buildIndex,
        int questionIndex,
        QuestionLevel questionLevel,
        int pageIndex,
        IList<IContent> content,
        string reference,
        IList<IContent> solve,
        string answer) : base(majorIndex, minorIndex, buildIndex, questionIndex, pageIndex, QuestionInputType.Calculation, questionLevel, content, reference)
    {
        Solve = solve;
        Answer = answer;
        IsValid = CheckValid();
    }

    protected bool CheckValid()
    {
        return CheckBasicValid();
    }

    public override bool IsValid { get; }

    protected string GetSolve()
    {
        var solveString = new StringBuilder();
        foreach (var item in Solve)
        {
            solveString.Append(item);
        }

        return solveString.ToString();
    }

    public override string ToString()
    {
        var baseString = new StringBuilder(base.ToString());
        baseString.Append('\n');


        baseString.Append($"答案: {Answer}");
        baseString.Append('\n');
        baseString.AppendLine($"解答: {GetSolve()}");
        baseString.AppendLine($"关联评价点名称: {Reference}");
        return baseString.ToString();
    }
}