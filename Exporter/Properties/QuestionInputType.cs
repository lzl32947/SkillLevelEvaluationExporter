using System.ComponentModel;

namespace SkillLevelEvaluationExporter.Properties;

public enum QuestionInputType
{
    [Description("单选题")]
    SingleSelection, // 单选题

    [Description("多选题")]
    MultipleSelection, // 多选题

    [Description("判断题")]
    TrueOrFalse, // 判断题

    [Description("计算题")]
    Calculation, // 计算题

    [Description("识图题")]
    PictureSelection, // 图片选择题

    [Description("未知")]
    Unknown // 未知
}