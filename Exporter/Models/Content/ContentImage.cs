using System.Drawing;
using SkillLevelEvaluationExporter.Models.Interfaces;
using SkillLevelEvaluationExporter.Utils;

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
        return Path.Combine(SaveDirectoryPath, SaveFileName);
    }

    public string SaveFileName { get;  }

    public string SaveDirectoryPath { get;  }

    public int ImageHeight { get; }

    public string Md5 { get; }

    public int ImageWidth { get; }

    public bool IsValidImage { get; }


    public ContentImage(string saveFileName, string saveDirectoryPath, int imageIndex)
    {
        Guid = Guid.NewGuid();
        SaveFileName = saveFileName;
        SaveDirectoryPath = saveDirectoryPath;
        if (!File.Exists(Path.Combine(saveDirectoryPath, SaveFileName)))
        {
            var errorPlaceHolder = Path.Combine(Environment.CurrentDirectory, "Resources", "error.png");
            if (File.Exists(errorPlaceHolder))
            {
                File.Copy(errorPlaceHolder, Path.Combine(saveDirectoryPath, saveFileName));
            }
            else
            {
                File.WriteAllBytes(Path.Combine(saveDirectoryPath, saveFileName), FileUtil.GetErrorPlaceholderImage());
            }
        }

        Md5 = FileUtil.CalculateMD5(Path.Combine(saveDirectoryPath, SaveFileName));
        ImageIndex = imageIndex;
        try
        {
            var image = Image.FromFile(Path.Combine(saveDirectoryPath, saveFileName));
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


    public override bool Equals(object? obj)
    {
        if (obj is ContentImage image)
        {
            return Md5 == image.Md5;
        }

        return false;
    }
}