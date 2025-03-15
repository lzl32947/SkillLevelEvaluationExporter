namespace SkillLevelEvaluationExporter.Models.Interfaces;

public interface IImageContent : IContent
{
    public int ImageIndex { get; }

    public string ToString();

    public string GetImageFilePath();

    public int ImageHeight { get;}

    public int ImageWidth { get;  }

    public bool IsValidImage { get; }
}