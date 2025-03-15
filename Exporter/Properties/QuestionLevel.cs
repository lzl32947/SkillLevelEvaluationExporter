using System.ComponentModel;

namespace SkillLevelEvaluationExporter.Properties;

public enum QuestionLevel
{
    [Description("初级工")]
    Level1, // 初级工

    [Description("中级工")]
    Level2, // 中级工


    [Description("高级工")]
    Level3, // 高级工

    [Description("技师")]
    Level4, // 技师

    [Description("高级技师")]
    Level5, // 高级技师

    [Description("未知")]
    Unknown // 未知
}