using System.Text;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
using SkillLevelEvaluationExporter.Properties;
using SkillLevelEvaluationExporter.Utils;

namespace SkillLevelEvaluationExporter.Models.Questions;

public abstract class Question
{
    private IList<IContent> _content = new List<IContent>();

    public IList<IContent> Content
    {
        get => _content;
        set
        {
            _content = value;
            ContentString = SetString(value);
        }
    }


    public QuestionLevel Level { get;  }

    public int MajorIndex { get; }

    public int MinorIndex { get; }

    public int BuildIndex { get; }

    public int QuestionIndex { get; }

    public int PageIndex { get; }

    public abstract bool IsValid { get; }

    public QuestionInputType InputType { get; }

    public string Reference { get; }

    public string ContentString { get; set; } = string.Empty;

    public string SetString(IList<IContent> content)
    {
        var builder = new StringBuilder();
        foreach (var item in content)
        {
            builder.Append(item);
        }

        return builder.ToString();
    }


    protected Question(int majorIndex , int minorIndex , int buildIndex, int questionIndex , int pageIndex , QuestionInputType inputType, QuestionLevel level ,
        IList<IContent> content , string reference = "")
    {
        MajorIndex = majorIndex;
        MinorIndex = minorIndex;
        BuildIndex = buildIndex;
        Level = level;
        QuestionIndex = questionIndex;
        PageIndex = pageIndex;
        InputType = inputType;
        Reference = reference;
        Content = content;
    }


    protected bool CheckBasicValid()
    {
        if (MajorIndex < 0 || MinorIndex < 0 || BuildIndex < 0 || QuestionIndex < 0 || PageIndex < 0)
        {
            return false;
        }

        if (InputType == QuestionInputType.Unknown)
        {
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        return $"""
                {MajorIndex}.{MinorIndex}.{BuildIndex} ${ReflectionUtil.GetEnumDescription(InputType)} 第{QuestionIndex}题
                {ContentString}
                """;
    }
}