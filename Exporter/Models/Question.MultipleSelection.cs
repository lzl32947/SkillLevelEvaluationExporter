﻿using System.Text;
using SkillLevelEvaluationExporter.Interfaces;
using SkillLevelEvaluationExporter.Properties;
using SkillLevelEvaluationExporter.Utils;

namespace SkillLevelEvaluationExporter.Models;

public class MultipleSelectionQuestion : Question
{
    public IList<IList<IContent>> Options { get; }

    public IList<int> AnswerIndex { get; }

    public MultipleSelectionQuestion(
        int majorIndex,
        int minorIndex,
        int buildIndex,
        int questionIndex,
        int pageIndex,
        string reference,
        IList<IList<IContent>> options,
        IList<int> answerIndex) : base(majorIndex, minorIndex, buildIndex, questionIndex, pageIndex, QuestionInputType.MultipleSelection, reference)
    {
        Options = options;
        AnswerIndex = answerIndex;
        IsValid = CheckValid();
    }

    protected bool CheckValid()
    {
        if (base.CheckBasicValid())
        {
            if (Options.Count < 2)
            {
                return false;
            }

            if ( AnswerIndex.Count == 0)
            {
                return false;
            }

            if (AnswerIndex.Any(index => index < 0 || index >= Options.Count))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public override bool IsValid { get; }

    public override string ToString()
    {
        var baseString = new StringBuilder(base.ToString());
        baseString.Append('\n');
        for (int index = 0; index < Options.Count; index++)
        {
            baseString.Append($"{QuestionUtil.Index2String(index)}. ");
            foreach (var item in Options[index])
            {
                baseString.Append(item);
            }

            baseString.Append('\n');
        }

        baseString.Append("答案: ");
        baseString.Append(string.Join("", AnswerIndex.Select(QuestionUtil.Index2String)));
        baseString.Append('\n');
        baseString.AppendLine($"关联评价点名称: {Reference}");
        return baseString.ToString();
    }
}