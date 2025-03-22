using System.Drawing;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
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

    public string ToMd5String()
    {
        return $"[图片:{Md5}]";
    }

    public string GetImageFilePath()
    {
        return Path.Combine(SaveDirectoryPath, SaveFileName);
    }

    public int? Height { get; }

    public int? Width { get; }

    public bool Valid { get; }

    public bool HasReplaced { get; }

    public string SaveFileName { get;  }

    public string SaveDirectoryPath { get;  }

    public string Md5 { get; }



    public ContentImage(string saveFileName, string saveDirectoryPath, int imageIndex)
    {
        Guid = Guid.NewGuid();
        SaveFileName = saveFileName;
        SaveDirectoryPath = saveDirectoryPath;
        var imageFilePath = Path.Combine(saveDirectoryPath, saveFileName);
        if (!File.Exists(imageFilePath))
        {
            Valid = false;
            var errorPlaceHolder = Path.Combine(Environment.CurrentDirectory, "Resources", "error.png");
            if (File.Exists(errorPlaceHolder))
            {
                File.Copy(errorPlaceHolder, imageFilePath);
            }
            else
            {
                File.WriteAllBytes(imageFilePath, FileUtil.GetErrorPlaceholderImage());
            }
            HasReplaced = true;
            Height = null;
            Width = null;
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
                HasReplaced = true;
            }
            else
            {
                using (var image = Image.FromFile(imageFilePath))
                {
                    Height = image.Height;
                    Width = image.Width;
                }
                Valid = true;
                HasReplaced = false;
            }

        }
        Md5 = FileUtil.CalculateMD5(imageFilePath);
        ImageIndex = imageIndex;
    }


    public override bool Equals(object? obj)
    {
        if (obj is ContentImage image)
        {
            return Md5 == image.Md5;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Md5.GetHashCode();
    }
}