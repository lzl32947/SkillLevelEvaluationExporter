using Exporter.Test.Source;
using SkillLevelEvaluationExporter.Utils;

namespace Exporter.Test.Utils;

public static class QuestionTestUtil
{
    public static List<QuestionIdentifier> FindNonContinuousSequences(List<(int a, int b, int c)> sequences)
    {
        var tupleList = QuestionUtil.FindNonContinuousSequences(sequences);
        return tupleList.Select(x => new QuestionIdentifier
        {
            Major = x.Item1,
            Minor = x.Item2,
            Build = x.Item3
        }).ToList();
    }

    public static List<QuestionIdentifier> FindNonContinuousSequences(List<QuestionIdentifier> identifiers)
    {
        var tupleList = QuestionUtil.FindNonContinuousSequences(identifiers.Select(x => (x.Major, x.Minor, x.Build)).ToList());
        return tupleList.Select(x => new QuestionIdentifier
        {
            Major = x.Item1,
            Minor = x.Item2,
            Build = x.Item3
        }).ToList();
    }
}