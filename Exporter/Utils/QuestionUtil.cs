// 命名空间SkillLevelEvaluationExporter.Utils包含了用于处理问题的工具类
namespace SkillLevelEvaluationExporter.Utils;

// QuestionUtil类提供了与问题索引和字符串表示转换相关的工具方法
public static class QuestionUtil
{
    /// <summary>
    /// 将整数索引转换为对应的字符串标识
    /// </summary>
    /// <param name="index">整数索引,通常用于标识问题的选项</param>
    /// <returns>对应的字符串标识,如"A", "B"等；如果索引超出范围,则返回"Unknown"</returns>
    public static string Index2String(int index)
    {
        return index switch
        {
            0 => "A",
            1 => "B",
            2 => "C",
            3 => "D",
            4 => "E",
            5 => "F",
            6 => "G",
            7 => "H",
            8 => "I",
            9 => "J",
            10 => "K",
            11 => "L",
            12 => "M",
            13 => "N",
            14 => "O",
            15 => "P",
            16 => "Q",
            17 => "R",
            18 => "S",
            19 => "T",
            20 => "U",
            21 => "V",
            22 => "W",
            23 => "X",
            24 => "Y",
            25 => "Z",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// 将整数索引列表转换为对应的字符串标识列表
    /// </summary>
    /// <param name="index">整数索引列表</param>
    /// <returns>对应的字符串标识列表</returns>
    public static IList<string> Index2String(IList<int> index)
    {
        return index.Select(Index2String).ToList();
    }

    /// <summary>
    /// 将整数索引列表转换为一个连续的字符串
    /// </summary>
    /// <param name="index">整数索引列表</param>
    /// <returns>一个连续的字符串,由各个索引对应的字符串标识组成</returns>
    public static string Index2JointString(IList<int> index)
    {
        return string.Join("", Index2String(index));
    }

    /// <summary>
    /// 将字符串标识转换为对应的整数索引
    /// </summary>
    /// <param name="str">字符串标识,如"A", "B"等</param>
    /// <returns>对应的整数索引；如果字符串不在定义范围内,则返回-1</returns>
    public static int String2Index(string str)
    {
        return str switch
        {
            "A" => 0,
            "B" => 1,
            "C" => 2,
            "D" => 3,
            "E" => 4,
            "F" => 5,
            "G" => 6,
            "H" => 7,
            "I" => 8,
            "J" => 9,
            "K" => 10,
            "L" => 11,
            "M" => 12,
            "N" => 13,
            "O" => 14,
            "P" => 15,
            "Q" => 16,
            "R" => 17,
            "S" => 18,
            "T" => 19,
            "U" => 20,
            "V" => 21,
            "W" => 22,
            "X" => 23,
            "Y" => 24,
            "Z" => 25,
            _ => -1
        };
    }

    /// <summary>
    /// 将字符串标识列表转换为对应的整数索引列表
    /// </summary>
    /// <param name="str">字符串标识列表</param>
    /// <returns>对应的整数索引列表</returns>
    public static IList<int> String2Index(IList<string> str)
    {
        return str.Select(String2Index).ToList();
    }

}
