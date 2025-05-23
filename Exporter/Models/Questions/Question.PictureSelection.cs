﻿using System.Text;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
using SkillLevelEvaluationExporter.Properties;
using SkillLevelEvaluationExporter.Utils;

namespace SkillLevelEvaluationExporter.Models.Questions;

public class PictureSelectionQuestion : Question
{
    public IList<IList<IContent>> Options { get; }

    public int AnswerIndex { get; }

    public PictureSelectionQuestion(
        int majorIndex,
        int minorIndex,
        int buildIndex,
        int questionIndex,
        QuestionLevel questionLevel,
        int pageIndex,
        IList<IContent> content,
        string reference,
        IList<IList<IContent>> options,
        int answerIndex) : base(majorIndex, minorIndex, buildIndex, questionIndex, pageIndex, QuestionInputType.PictureSelection, questionLevel, content, reference)
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
            baseString.Append($"{index.Index2String()}. ");
            foreach (var item in Options[index])
            {
                baseString.Append(item);
            }

            baseString.Append('\n');
        }

        baseString.Append($"答案: {AnswerIndex.Index2String()}");
        baseString.Append('\n');
        baseString.AppendLine($"关联评价点名称: {Reference}");
        return baseString.ToString();
    }
}