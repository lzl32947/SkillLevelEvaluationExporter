using System.Drawing;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
using SkillLevelEvaluationExporter.Utils;

namespace SkillLevelEvaluationExporter.Models.Content;

public class ContentImage : IImageContent
{
    public int ImageIndex { get; }

    public override string ToString()
    {
        return $"[图片:{ImageIndex:D4}]";
    }

    public string ToMd5String()
    {
        return $"[图片:{Md5??"默认"}]";
    }

    public string ToPlainString()
    {
        return "[图片]";
    }

    public string GetImageFilePath()
    {
        return Path.Combine(SaveDirectoryPath, SaveFileName);
    }

    public int? Height { get; }

    public int? Width { get; }

    public bool Valid { get; }

    public string SaveFileName { get;  }

    public string SaveDirectoryPath { get;  }

    public string? Md5 { get; }



    public ContentImage(string saveFileName, string saveDirectoryPath, int imageIndex)
    {
        SaveFileName = saveFileName;
        SaveDirectoryPath = saveDirectoryPath;
        var imageFilePath = Path.Combine(saveDirectoryPath, saveFileName);
        if (!File.Exists(imageFilePath))
        {
            Valid = false;
            Md5 = null;
            Height = null;
            Width = null;
            var errorPlaceHolder = Path.Combine(Environment.CurrentDirectory, "Resources", "error.png");
            if (File.Exists(errorPlaceHolder))
            {
                File.Copy(errorPlaceHolder, imageFilePath);
            }
            else
            {
                File.WriteAllBytes(imageFilePath, FileUtil.GetErrorPlaceholderImage());
            }
        }
        else
        {
            if (!FileUtil.IsValidImage(imageFilePath))
            {
                using (var image = Image.FromFile(imageFilePath))
                {
                    Height = image.Height;
                    Width = image.Width;
                }
                Valid = false;
                Md5 = null;
                var errorPlaceHolder = Path.Combine(Environment.CurrentDirectory, "Resources", "error.png");
                File.Delete(imageFilePath);
                if (File.Exists(errorPlaceHolder))
                {
                    File.Copy(errorPlaceHolder, imageFilePath);
                }
                else
                {
                    File.WriteAllBytes(imageFilePath, FileUtil.GetErrorPlaceholderImage());
                }
            }
            else
            {
                using (var image = Image.FromFile(imageFilePath))
                {
                    Height = image.Height;
                    Width = image.Width;
                }
                Valid = true;
                Md5 = FileUtil.CalculateMD5(imageFilePath);
            }
        }
        ImageIndex = imageIndex;
    }
}