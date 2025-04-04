using SkillLevelEvaluationExporter.Models.Content;
using SkillLevelEvaluationExporter.Models.Questions;
using SkillLevelEvaluationExporter.Properties;

namespace SkillLevelEvaluationExporter.Models;

public class Paper
{
    public string Title { get; }

    public DateTime PublishDate { get; }

    public IList<Question> Questions { get; }


    public string FileMd5 { get; }

    public string RawContent { get; }

    public IDictionary<Tuple<int, int>, QuestionLevel> LevelMap { get; }

    public IDictionary<int, QuestionInputType> TypeMap { get; }

    public IDictionary<Tuple<int, int, int>, int> PageMap { get; }

    public Paper(string title, DateTime publishDate, IList<Question> questions, string fileMd5, string rawContent, IDictionary<Tuple<int, int>, QuestionLevel> levelMap, IDictionary<int, QuestionInputType> typeMap,
        IDictionary<Tuple<int, int, int>, int> pageMap)
    {
        Title = title;
        PublishDate = publishDate;
        Questions = questions;
        FileMd5 = fileMd5;
        RawContent = rawContent;
        LevelMap = levelMap;
        TypeMap = typeMap;
        PageMap = pageMap;
    }

    public Paper(Paper other)
    {
        Title = other.Title;
        PublishDate = other.PublishDate;
        Questions = other.Questions;
        FileMd5 = other.FileMd5;
        RawContent = other.RawContent;
        LevelMap = new Dictionary<Tuple<int, int>, QuestionLevel>(other.LevelMap);
        TypeMap = new Dictionary<int, QuestionInputType>(other.TypeMap);
        PageMap = new Dictionary<Tuple<int, int, int>, int>(other.PageMap);
    }

}