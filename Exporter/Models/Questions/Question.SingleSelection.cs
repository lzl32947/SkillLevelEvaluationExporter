﻿using System.Text;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
using SkillLevelEvaluationExporter.Properties;
using SkillLevelEvaluationExporter.Utils;

namespace SkillLevelEvaluationExporter.Models.Questions;

public class SingleSelectionQuestion : Question
{
    public IList<IList<Object>> Options { get; }

    public int AnswerIndex { get; }


    public SingleSelectionQuestion(
        int majorIndex,
        int minorIndex,
        int buildIndex,
        int questionIndex,
        QuestionLevel questionLevel,
        int pageIndex,
        IList<Object> content,
        string reference,
        IList<IList<Object>> options,
        int answerIndex) : base(majorIndex, minorIndex, buildIndex, questionIndex, pageIndex, QuestionInputType.SingleSelection, questionLevel, content, reference)
    {
        Options = options;
        AnswerIndex = answerIndex;
        IsValid = CheckValid();
    }

    protected bool CheckValid()
    {
        if (CheckBasicValid())
        {
            if (Options.Count < 2)
            {
                return false;
            }

            if (AnswerIndex < 0 || AnswerIndex >= Options.Count)
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

        baseString.Append($"答案: {QuestionUtil.Index2String(AnswerIndex)}");
        baseString.Append('\n');
        baseString.AppendLine($"关联评价点名称: {Reference}");
        return baseString.ToString();
    }
}