// See https://aka.ms/new-console-template for more information

using CommandLine;

namespace Exporter.TestCreator;

public static class Program
{
    public class Options
    {
        [Option('s', "source", Required = true, HelpText = "输入文件（夹）路径")]
        public string Source { get; set; }

        [Option('o', "output", Required = false, HelpText = "输出文件夹路径")]
        public string? Output { get; set; }

    }

public static void WriteFileToOutput(string fileSource, string? output)
{
    // 校验输入路径是否合法
    if (string.IsNullOrWhiteSpace(fileSource))
    {
        throw new ArgumentException("文件路径不能为空或仅包含空白字符", nameof(fileSource));
    }

    try
    {
        // 获取文件相关信息并缓存结果
        var fileFullPath = Path.GetFullPath(fileSource);
        var fileName = Path.GetFileName(fileSource);
        var fileNameWithoutSuffix = Path.GetFileNameWithoutExtension(fileSource);

        // 检查文件是否存在
        if (!File.Exists(fileSource))
        {
            throw new FileNotFoundException($"指定的文件不存在: {fileSource}", fileSource);
        }

        // 如果指定了输出目录
        if (output != null)
        {
            // 校验输出路径是否合法
            if (string.IsNullOrWhiteSpace(output))
            {
                throw new ArgumentException("输出路径不能为空或仅包含空白字符", nameof(output));
            }

            // 确保输出目录存在
            var outputPath = Path.Combine(output, "FileSource.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

            // 写入文件内容
            File.AppendAllText(outputPath, $"{fileFullPath}\n");
        }
        else
        {
            // 直接输出到控制台
            Console.WriteLine($"{fileFullPath}");
        }
    }
    catch (ArgumentException ex)
    {
        Console.Error.WriteLine($"参数错误: {ex.Message}");
    }
    catch (FileNotFoundException ex)
    {
        Console.Error.WriteLine($"文件未找到: {ex.Message}");
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.Error.WriteLine($"目录未找到: {ex.Message}");
    }
    catch (UnauthorizedAccessException ex)
    {
        Console.Error.WriteLine($"权限不足: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"发生未知错误: {ex.Message}");
    }
}


    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts =>
            {
               var target = opts.Source;
                if (Directory.Exists(target))
                {
                    foreach (var file in Directory.GetFiles(target))
                    {
                        if (Path.GetExtension(file) == ".pdf")
                        {
                            WriteFileToOutput(file, opts.Output);
                        }
                    }
                }
                else if (File.Exists(target))
                {
                    WriteFileToOutput(target, opts.Output);
                }

            })
            .WithNotParsed<Options>((errs) =>
            {
                 Console.WriteLine("参数解析失败，请正确配置参数");
            });
        Console.WriteLine();
        Console.WriteLine("完成");
    }
}