using OneOf;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;

namespace SkillLevelEvaluationExporter.Models.Content;

public class PdfContentElement
{
    public PdfContentType ContentType { get;  }

    public PdfRectangle Rectangle { get;  }

    public string Content { get;  }

    public PdfContentElement(PdfContentType contentType, PdfRectangle rectangle, string content)
    {
        ContentType = contentType;
        Rectangle = rectangle;
        Content = content;
    }

}

public enum PdfContentType
{
    Text,
    Image
}