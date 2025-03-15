namespace SkillLevelEvaluationExporter.Models;

public class ExporterOptions
{
    // 小于该值的所有内容均被视为页面结尾
    public int YPageMin { get; set; } = 51;

    // 大于该值的所有内容均被视为页面开头
    public int YPageMax { get; set; } = 818;

    // 大于该值被认为是两行内容
    public int LineSep { get; set; } = 12;

    // 认为标题及日期应当在这些内容之前
    public int MaxTitleContentLength { get; set; } = 200;

}