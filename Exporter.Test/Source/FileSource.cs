namespace Exporter.Test.Source;

public static class FileSource
{
    public static IEnumerable<string[]> GetFiles()
    {
        var target = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "FileSource.txt");
        if (File.Exists(target))
        {
            var lines = File.ReadAllLines(target);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                yield return [line];
            }
        }
        else
        {
            Console.WriteLine("找不到文件：" + target);
        }
    }
}