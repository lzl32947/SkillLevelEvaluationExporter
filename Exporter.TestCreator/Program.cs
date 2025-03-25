// See https://aka.ms/new-console-template for more information

class Program
{
    static void WriteFileToConsole(string filePath)
    {
        using var file = File.Open(filePath, FileMode.Open);
        var fileName = Path.GetFileName(filePath);
        var fileFullPath = Path.GetFullPath(filePath);
        var fileNameWithoutSuffix = Path.GetFileNameWithoutExtension(filePath);
        Console.WriteLine($"{fileFullPath}");
    }

    static void Main(string[] args)
    {
        var target = args.Length > 0 ? args[0] : @"D:\Programs\VS\SkillLevelEvaluationExporter\Exporter.Test\Resources\PDF";
        if (Directory.Exists(target))
        {
            foreach (var file in Directory.GetFiles(target))
            {
                WriteFileToConsole(file);
            }
        }
        else if (File.Exists(target))
        {
            WriteFileToConsole(target);
        }
    }
}