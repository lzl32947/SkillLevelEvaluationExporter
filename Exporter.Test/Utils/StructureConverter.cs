using System.Collections;
using Exporter.Test.Source;
using SkillLevelEvaluationExporter.Models.Questions;
using SkillLevelEvaluationExporter.Properties;

namespace Exporter.Test.Utils;

public static class StructureConverter
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

    public static IList<QuestionIdentifier> ParseQuestionIdentifier(IList<Question> questions)
    {
        return questions.Select(q => new QuestionIdentifier
        {
            Major = q.MajorIndex,
            Minor = q.MinorIndex,
            Build = q.BuildIndex
        }).ToList();
    }

    public static Tuple<List<QuestionIdentifier>, List<QuestionIdentifier>> DifferenceQuestions(IList<QuestionIdentifier> a, IList<QuestionIdentifier> b)
    {
        var set1 = new HashSet<QuestionIdentifier>(a);
        var set2 = new HashSet<QuestionIdentifier>(b);

        if (set1.SetEquals(set2))
        {
            return new Tuple<List<QuestionIdentifier>, List<QuestionIdentifier>>([], []);
        }

        var differenceSet1 = new HashSet<QuestionIdentifier>(set1);
        differenceSet1.ExceptWith(set2);

        var differenceSet2 = new HashSet<QuestionIdentifier>(set2);
        differenceSet2.ExceptWith(set1);

        return Tuple.Create(differenceSet1.ToList(), differenceSet2.ToList());
    }

}