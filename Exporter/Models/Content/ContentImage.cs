using System.Drawing;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
using SkillLevelEvaluationExporter.Utils;

namespace SkillLevelEvaluationExporter.Models.Content;

public class ContentImage :IContent
{
    public int ImageIndex { get; }

    public override string ToString()
    {
        return $"[图片:{ImageIndex:D4}]";
    }

    public string ToPlainString()
    {
        return "[图片]";
    }

    public int? Height { get; }

    public int? Width { get; }

    public bool Valid { get; }

    public string LocalImagePath { get; }

    public string? Md5 { get; }



    public ContentImage(string localImagePath, int imageIndex)
    {
        LocalImagePath = localImagePath;
        if (!File.Exists(localImagePath))
        {
            Valid = false;
            Md5 = null;
            Height = null;
            Width = null;
        }
        else
        {
            if (!FileUtil.IsValidImage(localImagePath))
            {
                using (var image = Image.FromFile(localImagePath))
                {
                    Height = image.Height;
                    Width = image.Width;
                }
                Valid = false;
                Md5 = null;
            }
            else
            {
                using (var image = Image.FromFile(localImagePath))
                {
                    Height = image.Height;
                    Width = image.Width;
                }
                Valid = true;
                Md5 = FileUtil.CalculateMD5(localImagePath);
            }
        }
        ImageIndex = imageIndex;
    }
}