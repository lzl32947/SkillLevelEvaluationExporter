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
        return set1.SetEquals(set2);
    }

}