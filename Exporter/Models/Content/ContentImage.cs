using System.Drawing;
using SkillLevelEvaluationExporter.Models.Interfaces;

namespace SkillLevelEvaluationExporter.Models.Content;

public class ContentImage : IImageContent
{
    public Guid Guid { get; }

    public int ImageIndex { get; }

    public override string ToString()
    {
        return $"[图片:{ImageIndex:D4}]";
    }

    public string GetImageFilePath()
    {
        return Path.Combine(SaveFilePath, SaveFileName);
    }

    public string SaveFileName { get;  }

    public string SaveFilePath { get;  }

    public int ImageHeight { get; }

    public int ImageWidth { get; }

    public bool IsValidImage { get; }


    public ContentImage(string savePath, int imageIndex)
    {
        Guid = Guid.NewGuid();
        SaveFileName = Path.GetFileName(savePath);
        SaveFilePath = Path.GetDirectoryName(savePath);
        ImageIndex = imageIndex;
        try
        {
            var image = Image.FromFile(savePath);
            ImageHeight = image.Height;
            ImageWidth = image.Width;
            IsValidImage = true;
        } catch (Exception)
        {
            IsValidImage = false;
            ImageHeight = -1;
            ImageWidth = -1;
        }
    }

    public ContentImage(string saveFileName, string saveFilePath, int imageIndex)
    {
        Guid = Guid.NewGuid();
        SaveFileName = saveFileName;
        SaveFilePath = saveFilePath;
        ImageIndex = imageIndex;
        try
        {
            var image = Image.FromFile(Path.Combine(saveFilePath, saveFileName));
            ImageHeight = image.Height;
            ImageWidth = image.Width;
            IsValidImage = true;
        } catch (Exception)
        {
            IsValidImage = false;
            ImageHeight = -1;
            ImageWidth = -1;
        }

    }
}