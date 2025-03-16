using System.Drawing;
using System.Drawing.Imaging;
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

    public static byte[] GetErrorPlaceholderImage()
    {
        var base64PlaceHolder = """
                                data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII="
                                """;
        return Convert.FromBase64String(base64PlaceHolder);
    }

    public static bool IsValidImage(string filePath)
    {
        if (!File.Exists(filePath))
            return false;
        try
        {
            using (Bitmap bitmap = new Bitmap(filePath))
            {
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
                byte[] pixelData = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelData, 0, bytes);
                bitmap.UnlockBits(bmpData);

                // Check if all pixels are black (R = G = B = 0)
                for (int i = 0; i < pixelData.Length; i += 4) // 4 bytes per pixel (BGRA)
                {
                    if (pixelData[i] != 0 || pixelData[i + 1] != 0 || pixelData[i + 2] != 0) // Ignore alpha (i+3)
                    {
                        return true; // Found a non-black pixel
                    }
                }

                return false; // All pixels are black
            }
        }
        catch (Exception)
        {
            return false;
        }

    }
}