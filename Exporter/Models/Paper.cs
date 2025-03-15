namespace SkillLevelEvaluationExporter.Models;

public class Paper
{
    public string Title { get; }

    public DateTime PublishDate { get; }

    public IList<Question> Questions { get; }

    public string FileMd5 { get; }

    public Paper(string title, DateTime publishDate, IList<Question> questions, string fileMd5)
    {
        Title = title;
        PublishDate = publishDate;
        Questions = questions;
        FileMd5 = fileMd5;
    }
}