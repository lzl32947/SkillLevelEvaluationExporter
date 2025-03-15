using System.Security.Cryptography;

namespace SkillLevelEvaluationExporter.Utils;

public class FileUtil
{
    public static string CalculateMD5(string filePath)
    {
        using MD5 md5 = MD5.Create();
        using FileStream stream = File.OpenRead(filePath);
        byte[] hashBytes = md5.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public static async Task<string> CalculateMD5Async(string filePath)
    {
        using MD5 md5 = MD5.Create();
        using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        byte[] hashBytes = await Task.Run(() => md5.ComputeHash(stream));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public static async Task<bool> IsValidPdfFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return false;

        if (Path.GetExtension(filePath).ToLower() != ".pdf")
            return false;

        byte[] buffer = new byte[4];
        await using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4, true))
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, 4);
            if (bytesRead < 4)
                return false;
        }

        return buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46;
    }

    public static bool IsValidPdfFile(string filePath)
    {
        if (!File.Exists(filePath))
            return false;

        if (Path.GetExtension(filePath).ToLower() != ".pdf")
            return false;

        byte[] buffer = new byte[4];
        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            int bytesRead = stream.Read(buffer, 0, 4);
            if (bytesRead < 4)
                return false;
        }

        return buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46;
    }
}