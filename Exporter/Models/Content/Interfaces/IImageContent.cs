namespace SkillLevelEvaluationExporter.Models.Content.Interfaces;

public interface IImageContent : IContent
{
    public int ImageIndex { get; }

    public string ToString();

    public string ToMd5String();

    public string Md5 { get; }

    public string GetImageFilePath();

    public int? Height { get;}

    public int? Width { get;  }

    public bool Valid { get; }

    public bool HasReplaced { get; }
}