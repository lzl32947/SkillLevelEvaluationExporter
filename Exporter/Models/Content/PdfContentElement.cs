using OneOf;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;

namespace SkillLevelEvaluationExporter.Models.Content;

public class PdfContentElement
{
    public PdfContentType ContentType { get; set; }

    public PdfRectangle Rectangle { get; set; }

    public string Content { get; set; }

    public int PageIndex { get; set; }

    public PdfContentElement(PdfContentType contentType, PdfRectangle rectangle, string content, int pageIndex)
    {
        ContentType = contentType;
        Rectangle = rectangle;
        Content = content;
        PageIndex = pageIndex;
    }

}

public enum PdfContentType
{
    Text,
    Image
}