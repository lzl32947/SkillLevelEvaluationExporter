namespace SkillLevelEvaluationExporter.Models;

public class Paper
{
    public string Title { get; set; }

    public DateTime PublishDate { get; set; }

    public IList<Question> Questions { get; set; }

    public string Md5 { get; set; }

    public Paper(string title, DateTime publishDate, IList<Question> questions, string md5)
    {
        Title = title;
        PublishDate = publishDate;
        Questions = questions;
        Md5 = md5;
    }
}