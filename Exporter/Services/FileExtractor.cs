using System.Text;
using System.Text.RegularExpressions;
using SkillLevelEvaluationExporter.Models;
using SkillLevelEvaluationExporter.Models.Content;
using SkillLevelEvaluationExporter.Properties;
using SkillLevelEvaluationExporter.Services.Interfaces;
using SkillLevelEvaluationExporter.Utils;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace SkillLevelEvaluationExporter.Services;

public class FileExtractor : IFileExtractor
{
    private string? _filePath;

    public string? FilePath
    {
        get => _filePath;
        set
        {
            Init();
            EnsurePdf(value);
            EnsureMd5(value);
            _filePath = value;
        }
    }

    private string? _md5;

    public string? Md5
    {
        get => _md5;
        set => _md5 = value;
    }

    public string? WorkingDirectory { set; get; }

    public bool? ForceOverride { set; get; } = false;

    public ExporterOptions? Options { set; get; }

    private int _imageCounter = 0;

    public Paper? Extract()
    {
        EnsureCanExtract();

        # region Ensure working directory

        if (string.IsNullOrEmpty(WorkingDirectory))
        {
            throw new ArgumentException("Working directory is empty");
        }

        if (!Directory.Exists(WorkingDirectory))
        {
            Directory.CreateDirectory(WorkingDirectory);
        }

        var targetDirectory = Path.Combine(WorkingDirectory!, Md5!);
        if (Directory.Exists(targetDirectory))
        {
            if (ForceOverride == true)
            {
                Directory.Delete(targetDirectory, true);
            }
        }

        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        var imageDirectory = Path.Combine(targetDirectory, "images");

        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

        # endregion

        try
        {
            var content = new StringBuilder();
            var pageDictionary = new Dictionary<int, string>();
            using (PdfDocument pdfDocument = PdfDocument.Open(FilePath!))
            {
                var pages = pdfDocument.GetPages().ToList();
                for (int pageNumIndex = 0; pageNumIndex < pages.Count(); pageNumIndex++)
                {
                    var wordList = new List<PdfContentElement>();
                    var page = pages[pageNumIndex];
                    var pageString = new StringBuilder();
                    // 提取文字
                    foreach (var word in page.GetWords())
                    {
                        var boundingBox = word.BoundingBox;
                        var rotation = word.TextOrientation;

                        // 过滤掉页面中的斜字
                        if (rotation == TextOrientation.Other)
                        {
                            continue;
                        }
                        // 过滤出页尾的第XX页

                        if (boundingBox.Bottom < Options!.YPageMin)
                        {
                            continue;
                        }

                        // 过滤出页头的工种定义
                        if (boundingBox.Top > Options!.YPageMax)
                        {
                            continue;
                        }

                        wordList.Add(new PdfContentElement(
                            PdfContentType.Text, boundingBox, word.Text, pageNumIndex)
                        );

                        pageString.AppendLine(word.Text);
                    }

                    pageDictionary.Add(pageNumIndex + 1, pageString.ToString());
                    // 提取图像
                    foreach (var image in page.GetImages())
                    {
                        var fileName = $"{_imageCounter:D4}";
                        var imageFilePath = Path.Combine(imageDirectory, fileName + ".png");
                        File.WriteAllBytes(imageFilePath, image.RawBytes.ToArray());
                        var boundingBox = image.Bounds;

                        wordList.Add(new PdfContentElement(
                            PdfContentType.Image, boundingBox, fileName, pageNumIndex)
                        );
                        _imageCounter++;
                    }


                    // 按照行高和左边界排序从而合并
                    wordList.Sort((a, b) =>
                    {
                        // 小于行高则保留原序列
                        if (a.Rectangle.Top < b.Rectangle.Top && Math.Abs(a.Rectangle.Top - b.Rectangle.Top) > Options!.LineSep)
                        {
                            return 1;
                        }
                        // 小于行高则保留原序列

                        if (a.Rectangle.Top > b.Rectangle.Top && Math.Abs(a.Rectangle.Top - b.Rectangle.Top) > Options!.LineSep)
                        {
                            return -1;
                        }

                        return a.Rectangle.Left.CompareTo(b.Rectangle.Left);
                    });

                    // 构造当前页面的文本数据
                    StringBuilder builder = new();

                    double previousTop = Math.Max(wordList[0].Rectangle.Top, Options!.YPageMin);

                    for (int i = 0; i < wordList.Count; i++)
                    {
                        PdfContentElement elementI = wordList[i];
                        if (Math.Abs(elementI.Rectangle.Top - previousTop) > Options!.LineSep)
                        {
                            // 两行内容
                            builder.Append('\n');
                            previousTop = elementI.Rectangle.Top;
                        }

                        if (elementI.ContentType == PdfContentType.Text)
                        {
                            builder.Append(elementI.Content);
                        }
                        else if (elementI.ContentType == PdfContentType.Image)
                        {
                            builder.Append($"[图片:{elementI.Content}]");
                        }
                    }

                    content = content.Append(builder);
                    content.AppendLine();
                }
            }

            // 处理页码
            var pageMap = new Dictionary<Tuple<int,int,int>, int>();
            foreach (var page in pageDictionary)
            {
                var pageIndex = page.Key;
                var contentText = page.Value;
                var pattern = @"^(\d+)\.(\d+)\.(\d+)";
                var regex = new Regex(pattern, RegexOptions.Multiline);
                var matches = regex.Matches(contentText);
                foreach (Match match in matches)
                {
                    var majorIndex = int.Parse(match.Groups[1].Value);
                    var minorIndex = int.Parse(match.Groups[2].Value);
                    var buildIndex = int.Parse(match.Groups[3].Value);
                    pageMap.Add(new Tuple<int, int, int>(majorIndex, minorIndex, buildIndex), pageIndex);
                }
            }

            var inputTypes = ReadMajor(content.ToString());
            var questionLevels = ReadMinor(content.ToString());
            var meta = ReadMeta(content.ToString());
            var questions = ReadQuestions(content.ToString());

            var questionTotal = new List<Question?>();

            for (int index = 0; index < questions.Count; index++)
            {
                var question = questions[index];
                var majorIndex = question.Item1;
                var minorIndex = question.Item2;
                var buildIndex = question.Item3;
                var contentText = question.Item4;
                var level = questionLevels[new Tuple<int, int>(majorIndex, minorIndex)];
                var type = inputTypes[majorIndex];
                pageMap.TryGetValue(new Tuple<int, int, int>(majorIndex, minorIndex, buildIndex),out var page);
                questionTotal.Add(QuestionFactory.Create(contentText, type, level,page, imageDirectory));
            }


            var paper = new Paper(meta.Item1, new DateTime(year: meta.Item2.Item1, month: meta.Item2.Item2, day: meta.Item2.Item3),
                questionTotal.Where(q => q != null).ToList()!, Md5!
            );
            return paper;
        }
        catch (Exception)
        {
            try
            {
                Directory.Delete(targetDirectory, true);
            }
            catch (Exception)
            {
                // ignored
            }

            throw;
        }

        return null;
    }


    public IDictionary<int, QuestionInputType> ReadMajor(string contentText)
    {
        string pattern = @"^(\d)\.([^0-9]*?)\d+$";
        Regex regex = new(pattern, RegexOptions.Multiline);
        MatchCollection matches = regex.Matches(contentText);
        IDictionary<int, string> catalog = new Dictionary<int, string>();
        foreach (Match match in matches)
        {
            int order = int.Parse(match.Groups[1].Value);
            string content = match.Groups[2].Value;
            catalog.Add(order, content);
        }

        Dictionary<int, QuestionInputType> typeMap = new Dictionary<int, QuestionInputType>();

        foreach (var (key, value) in catalog)
        {
            if (value.Contains("单选题"))
            {
                typeMap.Add(key, QuestionInputType.SingleSelection);
            }
            else if (value.Contains("多选题"))
            {
                typeMap.Add(key, QuestionInputType.MultipleSelection);
            }
            else if (value.Contains("判断题"))
            {
                typeMap.Add(key, QuestionInputType.TrueOrFalse);
            }
            else if (value.Contains("识图题"))
            {
                typeMap.Add(key, QuestionInputType.PictureSelection);
            }
            else if (value.Contains("计算题"))
            {
                typeMap.Add(key, QuestionInputType.Calculation);
            }
            else
            {
                typeMap.Add(key, QuestionInputType.Unknown);
            }
        }

        return typeMap;
    }

    public IDictionary<Tuple<int, int>, QuestionLevel> ReadMinor(string contentText)
    {
        string pattern = @"^(\d+)\.(\d+)\.([^0-9]+?)\d+\s*$";
        Regex regex = new(pattern, RegexOptions.Multiline);
        MatchCollection matches = regex.Matches(contentText);
        IDictionary<Tuple<int, int>, string> menu = new Dictionary<Tuple<int, int>, string>();
        foreach (Match match in matches)
        {
            int orderSmall = int.Parse(match.Groups[1].Value);
            int orderMid = int.Parse(match.Groups[2].Value);
            string content = match.Groups[3].Value;
            menu.Add(new Tuple<int, int>(orderSmall, orderMid), content);
        }

        Dictionary<Tuple<int, int>, QuestionLevel> levelMap = new Dictionary<Tuple<int, int>, QuestionLevel>();

        foreach (var (key, value) in menu)
        {
            if (value.Contains("初级工"))
            {
                levelMap.Add(key, QuestionLevel.Level1);
            }
            else if (value.Contains("中级工"))
            {
                levelMap.Add(key, QuestionLevel.Level2);
            }
            else if (value.Contains("高级工"))
            {
                levelMap.Add(key, QuestionLevel.Level3);
            }
            else if (value.Equals("技师"))
            {
                levelMap.Add(key, QuestionLevel.Level4);
            }
            else if (value.Equals("高级技师"))
            {
                levelMap.Add(key, QuestionLevel.Level5);
            }
            else
            {
                levelMap.Add(key, QuestionLevel.Unknown);
            }
        }

        return levelMap;
    }


    public Tuple<string, Tuple<int, int, int>> ReadMeta(string contentText)
    {
        string pattern = @"^(.*)\s技能等级评价机考题库[.\S\s]*?(\d\d\d\d)年(\d+)月(\d+)日";
        Regex regex = new(pattern);
        Match match = regex.Match(contentText.Length > Options!.MaxTitleContentLength ? contentText.Substring(0, Options!.MaxTitleContentLength) : contentText);
        if (match.Success)
        {
            string title = match.Groups[1].Value;
            string year = match.Groups[2].Value;
            string month = match.Groups[3].Value;
            string day = match.Groups[4].Value;
            return new Tuple<string, Tuple<int, int, int>>(title, new Tuple<int, int, int>(int.Parse(year), int.Parse(month), int.Parse(day)));
        }
        else
        {
            throw new ApplicationException($"Can not parse the title and corresponding time!");
        }
    }

    public IList<Tuple<int, int, int, string>> ReadQuestions(string contentText)
    {
        List<Tuple<int, int, int, string>> list = new();
        string pattern = @"(^(\d+)\.(\d+)\.(\d+)[.\S\s]*?(?=^\d+\.\d+\.))";
        Regex regex = new(pattern, RegexOptions.Multiline);
        // 999.999.999 为了匹配最后一个题目
        MatchCollection matches = regex.Matches(contentText + "\n999.999.999");
        foreach (Match match in matches)
        {
            var content = match.Groups[1].Value;
            var major = int.Parse(match.Groups[2].Value);
            var minor = int.Parse(match.Groups[3].Value);
            var build = int.Parse(match.Groups[4].Value);
            list.Add(new Tuple<int, int, int, string>(major, minor, build, content));
        }

        return list;
    }

    private void EnsureCanExtract()
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            throw new ArgumentException("PDF file path is empty");
        }

        if (string.IsNullOrEmpty(WorkingDirectory))
        {
            throw new ArgumentException("Working directory is empty");
        }

        if (string.IsNullOrEmpty(Md5))
        {
            throw new ArgumentException("MD5 is empty");
        }

        if (Options == null)
        {
            throw new ArgumentException("Options is empty");
        }
    }

    private void Init()
    {
        Options ??= new ExporterOptions();
        _imageCounter = 0;
        Md5 = null;
    }

    private void EnsurePdf(string? pdfFilePath)
    {
        if (string.IsNullOrEmpty(pdfFilePath))
        {
            throw new ArgumentException("PDF file path is empty");
        }

        var flag = FileUtil.IsValidPdfFile(pdfFilePath);
        if (!flag)
        {
            throw new ArgumentException("Invalid PDF file");
        }
    }

    private void EnsureMd5(string? pdfFilePath)
    {
        if (string.IsNullOrEmpty(pdfFilePath))
        {
            throw new ArgumentException("PDF file path is empty");
        }

        var md5 = FileUtil.CalculateMD5(pdfFilePath);
        Md5 = md5;
    }
}