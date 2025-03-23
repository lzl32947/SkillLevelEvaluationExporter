using SkillLevelEvaluationExporter.Models.Questions;
using SkillLevelEvaluationExporter.Properties;

namespace Exporter.Test.Utils;

public static class TestStructureConverter
{
    public static IList<int[]> LevelMapToArray(IDictionary<Tuple<int, int>, QuestionLevel> levelMap)
    {
        return levelMap.Select(x => new int[] { x.Key.Item1, x.Key.Item2, (int)x.Value }).ToList();
    }

    public static IDictionary<Tuple<int, int>, QuestionLevel> ArrayToLevelMap(IList<int[]> levelMap)
    {
        return levelMap.ToDictionary(x => new Tuple<int, int>(x[0], x[1]), x => (QuestionLevel)x[2]);
    }

    public static IList<int[]> TypeMapToArray(IDictionary<int, QuestionInputType> typeMap)
    {
        return typeMap.Select(x => new int[] { x.Key, (int)x.Value }).ToList();
    }

    public static IDictionary<int, QuestionInputType> ArrayToTypeMap(IList<int[]> typeMap)
    {
        return typeMap.ToDictionary(x => x[0], x => (QuestionInputType)x[1]);
    }

    public static IList<int[]> PageMapToArray(IDictionary<Tuple<int, int, int>, int> pageMap)
    {
        return pageMap.Select(x => new int[] { x.Key.Item1, x.Key.Item2, x.Key.Item3, x.Value }).ToList();
    }

    public static IDictionary<Tuple<int, int, int>, int> ArrayToPageMap(IList<int[]> pageMap)
    {
        return pageMap.ToDictionary(x => new Tuple<int, int, int>(x[0], x[1], x[2]), x => x[3]);
    }

    public static IList<int[]> QuestionIdentifierToArray(IList<Question> questions)
    {
        return questions.Select(q => new int[] { q.MajorIndex, q.MinorIndex, q.BuildIndex }).ToList();
    }

    public static bool IsSameArray(IList<int[]> a, IList<int[]> b)
    {
        HashSet<string> set1 = new HashSet<string>(a.Select(arr => string.Join(",", arr)));
        HashSet<string> set2 = new HashSet<string>(b.Select(arr => string.Join(",", arr)));

        if (set1.SetEquals(set2))
        {
            return true;
        }

        HashSet<string> differenceSet1 = new HashSet<string>(set1);
        differenceSet1.ExceptWith(set2);

        HashSet<string> differenceSet2 = new HashSet<string>(set2);
        differenceSet2.ExceptWith(set1);

        Console.WriteLine($"在列表A中，在列表B中不存在的元素：{string.Join(", ", differenceSet1)}");
        Console.WriteLine($"在列表B中，在列表A中不存在的元素：{string.Join(", ", differenceSet2)}");
        return false;
    }

}