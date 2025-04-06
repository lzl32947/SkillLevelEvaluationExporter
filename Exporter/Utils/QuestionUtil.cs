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
    public static string Index2String(this int index)
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
    public static IList<string> Index2String(this IList<int> index)
    {
        return index.Select(Index2String).ToList();
    }

    /// <summary>
    /// 将整数索引列表转换为一个连续的字符串
    /// </summary>
    /// <param name="index">整数索引列表</param>
    /// <returns>一个连续的字符串,由各个索引对应的字符串标识组成</returns>
    public static string Index2JointString(this IList<int> index)
    {
        return string.Join("", Index2String(index));
    }

    /// <summary>
    /// 将字符串标识转换为对应的整数索引
    /// </summary>
    /// <param name="str">字符串标识,如"A", "B"等</param>
    /// <returns>对应的整数索引；如果字符串不在定义范围内,则返回-1</returns>
    public static int String2Index(this string str)
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

            "a" => 0,
            "b" => 1,
            "c" => 2,
            "d" => 3,
            "e" => 4,
            "f" => 5,
            "g" => 6,
            "h" => 7,
            "i" => 8,
            "j" => 9,
            "k" => 10,
            "l" => 11,
            "m" => 12,
            "n" => 13,
            "o" => 14,
            "p" => 15,
            "q" => 16,
            "r" => 17,
            "s" => 18,
            "t" => 19,
            "u" => 20,
            "v" => 21,
            "w" => 22,
            "x" => 23,
            "y" => 24,
            "z" => 25,
            _ => -1,
        };
    }

    /// <summary>
    /// 将字符串标识列表转换为对应的整数索引列表
    /// </summary>
    /// <param name="str">字符串标识列表</param>
    /// <returns>对应的整数索引列表</returns>
    public static IList<int> String2Index(this IList<string> str)
    {
        return str.Select(String2Index).ToList();
    }

    /// <summary>
    /// 检查一系列三元组序列中的值是否连续。
    /// </summary>
    /// <param name="sequences">包含三元组的列表，每个三元组包含三个整数(a, b, c)。</param>
    /// <returns>如果所有三元组序列中的c值对于每个独特的(a, b)对都是连续的，则返回true；否则返回false。</returns>
    public static bool CheckValuesContinuous(List<(int a, int b, int c)> sequences)
    {
        // 创建一部字典，将(a, b)对作为键，将对应的c值集合作为值
        var abToCs = new Dictionary<(int a, int b), HashSet<int>>();

        // 遍历序列，将每个 (a, b) 对应的 c 值添加到集合中
        foreach (var (a, b, c) in sequences)
        {
            // 如果字典中不存在该(a, b)键，则添加新的HashSet
            if (!abToCs.ContainsKey((a, b)))
            {
                abToCs[(a, b)] = new HashSet<int>();
            }
            // 将c值添加到对应(a, b)键的HashSet中
            abToCs[(a, b)].Add(c);
        }

        // 检查每个 (a, b) 对的 c 值集合是否连续
        foreach (var kvp in abToCs)
        {
            var cValues = kvp.Value;
            int minC = cValues.Min();
            int maxC = cValues.Max();

            // 通过遍历检查从 minC 到 maxC 的每个值是否存在
            for (int i = minC; i <= maxC; i++)
            {
                // 如果集合中缺少某个值，则序列不连续，返回false
                if (!cValues.Contains(i))
                {
                    return false;
                }
            }
        }

        // 所有检查都通过，证明所有序列连续，返回true
        return true;
    }

    /// <summary>
    /// 查找一系列三元组序列中不连续的编码。
    /// </summary>
    /// <param name="sequences">包含三元组的列表，每个三元组包含三个整数(a, b, c)。</param>
    /// <returns>一部字典，包含所有不连续的(a, b)对及其对应的不连续的c值集合。</returns>
    public static Dictionary<(int a, int b), HashSet<int>> FindNonContinuousDictionary(List<(int a, int b, int c)> sequences)
    {
        var abToCs = new Dictionary<(int a, int b), HashSet<int>>();
        var nonContinuousSequences = new Dictionary<(int a, int b), HashSet<int>>();

        // 将每个 (a, b) 对应的 c 值添加到集合中
        foreach (var (a, b, c) in sequences)
        {
            if (!abToCs.ContainsKey((a, b)))
            {
                abToCs[(a, b)] = new HashSet<int>();
            }
            abToCs[(a, b)].Add(c);
        }

        // 检查每个 (a, b) 对的 c 值集合是否连续
        foreach (var kvp in abToCs)
        {
            var cValues = kvp.Value;
            int minC = cValues.Min();
            int maxC = cValues.Max();

            // 通过遍历检查从 minC 到 maxC 的每个值是否存在
            for (int i = minC; i <= maxC; i++)
            {
                // 如果集合中缺少某个值，则序列不连续，记录该(a, b)对
                if (!cValues.Contains(i))
                {
                    nonContinuousSequences[kvp.Key] = kvp.Value;
                    break;
                }
            }
        }

        return nonContinuousSequences;
    }

    /// <summary>
    /// 查找并返回所有在给定序列中不连续的三元组。
    /// </summary>
    /// <param name="sequences">一个包含三元组的列表，每个三元组由三个整数组成。</param>
    /// <returns>返回一个列表，包含所有不连续的三元组，每个三元组为一个 Tuple。</returns>
    public static List<Tuple<int, int, int>> FindNonContinuousSequences(List<(int a, int b, int c)> sequences)
    {
        // 首先调用 FindNonContinuousDictionary 方法，获取不连续序列的字典表示
        var nonContinuousDictionary = FindNonContinuousDictionary(sequences);
        // 初始化结果列表，用于存储最终的不连续三元组
        var result = new List<Tuple<int, int, int>>();

        // 遍历字典中的每个键值对
        foreach (var kvp in nonContinuousDictionary)
        {
            // 解构键值对的键，获取 a 和 b 的值
            var (a, b) = kvp.Key;
            // 获取与当前 (a, b) 对对应的 c 值列表
            var cValues = kvp.Value;

            // 将每个 (a, b) 对及其对应的 c 值组合成 Tuple 并添加到结果列表中
            foreach (var c in cValues)
            {
                result.Add(new Tuple<int, int, int>(a, b, c));
            }
        }

        // 返回结果列表
        return result;
    }

}
