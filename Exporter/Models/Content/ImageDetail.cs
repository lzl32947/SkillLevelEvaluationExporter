namespace SkillLevelEvaluationExporter.Models.Content;

public class ImageDetail
{
    public int ImagePage { get; }
    public int PageImageIndex { get; }
    public int ImageIndex { get; }

    public ImageDetail(int imageIndex, int imagePage, int pageImageIndex)
    {
        ImagePage = imagePage;
        PageImageIndex = pageImageIndex;
        ImageIndex = imageIndex;
    }
}