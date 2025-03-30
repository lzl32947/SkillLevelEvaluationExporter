using System.Text.RegularExpressions;
using SkillLevelEvaluationExporter.Models.Content;
using SkillLevelEvaluationExporter.Models.Content.Interfaces;
using SkillLevelEvaluationExporter.Properties;

namespace SkillLevelEvaluationExporter.Models.Questions;

public static class QuestionFactory
{
    public static Question? Create(string question, QuestionInputType questionType, QuestionLevel questionLevel, int pageIndex, string imageDirectory)
    {
        switch (questionType)
        {
            case QuestionInputType.SingleSelection:
                return CreateSelectableQuestion(question, QuestionInputType.SingleSelection, questionLevel, pageIndex, imageDirectory) as SingleSelectionQuestion;
            case QuestionInputType.MultipleSelection:
                return CreateSelectableQuestion(question, QuestionInputType.MultipleSelection, questionLevel, pageIndex, imageDirectory) as MultipleSelectionQuestion;
            case QuestionInputType.PictureSelection:
                return CreateSelectableQuestion(question, QuestionInputType.PictureSelection, questionLevel, pageIndex, imageDirectory) as PictureSelectionQuestion;
            case QuestionInputType.TrueOrFalse:
                return CreateSelectableQuestion(question, QuestionInputType.TrueOrFalse, questionLevel, pageIndex, imageDirectory) as TrueOrFalseQuestion;
            case QuestionInputType.Calculation:
                return CreateCalculationQuestion(question, questionLevel, pageIndex, imageDirectory) as CalculationQuestion;
            default:
                return null;
        }
    }

    private static Question? CreateCalculationQuestion(string question, QuestionLevel level, int pageIndex, string imageDirectory)
    {
        string pattern = @"(\d+)\.(\d+)\.(\d+)\.\s*第(\d+)题\s([\s\S]*?)正确答案：(.*?)[,，]\s+教师详解：([\s\S]*?)关联评价点的名称：([.\s\S]*)";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(question);
        if (match.Success)
        {
            var major = int.Parse(match.Groups[1].Value);
            var minor = int.Parse(match.Groups[2].Value);
            var build = int.Parse(match.Groups[3].Value);
            var index = int.Parse(match.Groups[4].Value);
            var contentString = match.Groups[5].Value;
            var correctAnswer = match.Groups[6].Value;
            var reason = match.Groups[7].Value;
            var reference = match.Groups[8].Value;
            return new CalculationQuestion(
                major,
                minor,
                build,
                index,
                level,
                pageIndex,
                ParseContent(contentString, imageDirectory),
                reference.Trim('\n', '\r'),
                ParseContent(reason, imageDirectory),
                correctAnswer
            );
        }

        return null;
    }

    public static IList<IContent> ParseContent(string contentText, string imageDirectory)
    {
        List<Tuple<string, PdfContentType>> result = new();

        string pattern = @"(\[图片:\d+\])";
        string patternDigit = @"\[图片:(\d+)\]";

        var parts = Regex.Split(contentText, pattern);

        foreach (string part in parts)
        {
            if (!string.IsNullOrWhiteSpace(part))
            {
                if (Regex.IsMatch(part, patternDigit))
                {
                    Match match = Regex.Match(part, patternDigit);
                    if (match.Success)
                    {
                        result.Add(Tuple.Create(match.Groups[1].Value, PdfContentType.Image));
                    }
                }
                else
                {
                    result.Add(Tuple.Create(part.Trim('\n', '\r'), PdfContentType.Text));
                }
            }
        }

        return result.Select<Tuple<string, PdfContentType>, IContent>(tuple => tuple.Item2 switch
        {
            PdfContentType.Text => new ContentText(tuple.Item1),
            PdfContentType.Image => new ContentImage($"{tuple.Item1}.png", imageDirectory, Int32.Parse(tuple.Item1)),
            _ => throw new ArgumentOutOfRangeException()
        }).ToList();
    }

    public static Question? CreateSelectableQuestion(string questionContent, QuestionInputType inputType, QuestionLevel level, int pageIndex, string imageDirectory)
    {
        string pattern =
            @"^(\d+)\.(\d+)\.(\d+)\.第(\d+)题([\s\S]+?)(?=A)(?:A\.)([\s\S\n\r]+?)(?=B)(?:B\.)([\s\S\n\r]+?)(?=[C正])((?:C\.)([\s\S\n\r]+?)(?=[D正]))*((?:D\.)([\s\S\n\r]+?)(?=[E正]))*((?:E\.)([\s\S\n\r]+?)(?=[F正]))*((?:F\.)([\s\S\n\r]+?)(?=[G正]))*((?:G\.)([\s\S\n\r]+?)(?=H正))*((?:H\.)([\s\S\n\r]+?)(?=I正))*((?:I\.)([\s\S\n\r]+?)(?=正J))*((?:J\.)([\s\S\n\r]+?)(?=K正))*((?:K\.)([\s\S\n\r]+?)(?=正L))*((?:L\.)([\s\S\n\r]+?)(?=正M))*((?:M\.)([\s\S\n\r]+?)(?=正N))*((?:N\.)([\s\S\n\r]+?)(?=正))*正确答案：\s*([AaBbCcDdEeFfGgHhIiJjKkLlMmNn，ＡＢＣＤＥＦＧＨＩ]+)\s+关联评价点的名称：([.\s\S]*)";
        Regex regex = new(pattern);
        Match match = regex.Match(questionContent);
        if (match.Success)
        {
            var major = int.Parse(match.Groups[1].Value);
            var minor = int.Parse(match.Groups[2].Value);
            var build = int.Parse(match.Groups[3].Value);
            var index = int.Parse(match.Groups[4].Value);
            var contentString = match.Groups[5].Value;
            var choiceList = new List<string>();
            var correctList = new List<int>();
            // 选项 A
            choiceList.Add(match.Groups[6].Value);
            // 选项 B
            choiceList.Add(match.Groups[7].Value);
            // 选项 C to H
            for (int i = 9; i < 32; i += 2)
            {
                if (match.Groups[i].Success)
                {
                    choiceList.Add(match.Groups[i].Value);
                }
            }

            foreach (char c in match.Groups[32].Value)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    correctList.Add(c - 'A');
                }
                else if (c >= 'a' && c <= 'z')
                {
                    correctList.Add(c - 'a');
                }
                else if (c == 'Ａ')
                {
                    correctList.Add(0);
                }
                else if (c == 'Ｂ')
                {
                    correctList.Add(1);
                }
                else if (c == 'Ｃ')
                {
                    correctList.Add(2);
                }
                else if (c == 'Ｄ')
                {
                    correctList.Add(3);
                }
                else if (c == 'Ｅ')
                {
                    correctList.Add(4);
                }
                else if (c == 'Ｆ')
                {
                    correctList.Add(5);
                }
                else if (c == 'Ｇ')
                {
                    correctList.Add(6);
                }
                else if (c == 'Ｈ')
                {
                    correctList.Add(7);
                }
                else if (c == 'Ｉ')
                {
                    correctList.Add(8);
                }
            }

            var reference = match.Groups[33].Value;


            if (inputType == QuestionInputType.SingleSelection)
            {
                var options = new List<IList<IContent>>();
                foreach (var choice in choiceList)
                {
                    options.Add(ParseContent(choice, imageDirectory));
                }

                return new SingleSelectionQuestion(major, minor, build, index, level, pageIndex, ParseContent(contentString, imageDirectory), reference.Trim('\n', '\r'), options, correctList[0]);
            }

            if (inputType == QuestionInputType.MultipleSelection)
            {
                var options = new List<IList<IContent>>();
                foreach (var choice in choiceList)
                {
                    options.Add(ParseContent(choice, imageDirectory));
                }

                return new MultipleSelectionQuestion(major, minor, build, index, level, pageIndex, ParseContent(contentString, imageDirectory), reference.Trim('\n', '\r'), options, correctList);
            }

            if (inputType == QuestionInputType.PictureSelection)
            {
                var options = new List<IList<IContent>>();
                foreach (var choice in choiceList)
                {
                    options.Add(ParseContent(choice, imageDirectory));
                }

                return new PictureSelectionQuestion(major, minor, build, index, level, pageIndex, ParseContent(contentString, imageDirectory), reference.Trim('\n', '\r'), options, correctList[0]);
            }

            if (inputType == QuestionInputType.TrueOrFalse)
            {
                return new TrueOrFalseQuestion(major, minor, build, index, level, pageIndex, ParseContent(contentString, imageDirectory), reference.Trim('\n', '\r'), correctList[0] == 0);
            }

            throw new NotImplementedException("Question type not implemented");
        }

        return null;
    }
}