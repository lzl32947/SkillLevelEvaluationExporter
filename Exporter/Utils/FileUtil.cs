using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;

namespace SkillLevelEvaluationExporter.Utils;

public static class FileUtil
{
    /// <summary>
    /// 计算指定文件的MD5哈希值.
    /// </summary>
    /// <param name="filePath">要计算MD5哈希值的文件路径.</param>
    /// <returns>文件的MD5哈希值,以十六进制字符串形式返回.</returns>
    /// <exception cref="FileNotFoundException">如果指定的文件不存在.</exception>
    public static string CalculateMD5(string filePath)
    {
        // 创建MD5哈希对象
        using var md5 = MD5.Create();

        // 检查文件是否存在,如果不存在则抛出异常
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("未找到文件: ", filePath);
        }

        // 打开文件以读取内容
        using var stream = File.OpenRead(filePath);

        // 计算文件的MD5哈希值
        byte[] hashBytes = md5.ComputeHash(stream);

        // 将哈希值转换为十六进制字符串,移除连字符,并转换为小写
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 计算给定字符串的MD5哈希值。
    /// </summary>
    /// <param name="input">要计算哈希值的字符串。</param>
    /// <returns>字符串的MD5哈希值。</returns>
    public static string CalculateStringMd5(string input)
    {
        // 创建MD5实例
        using MD5 md5 = MD5.Create();
        // 将输入字符串转换为字节数组
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        // 计算哈希值
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // 将哈希值转换为十六进制字符串
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 异步计算给定文件的MD5哈希值.
    /// </summary>
    /// <param name="filePath">要计算MD5哈希值的文件路径.</param>
    /// <returns>返回文件的MD5哈希值.</returns>
    /// <exception cref="FileNotFoundException">如果指定的文件不存在.</exception>
    public static async Task<string> CalculateMD5Async(string filePath)
    {
        // 创建MD5哈希对象
        using var md5 = MD5.Create();

        // 检查文件是否存在,如果不存在则抛出异常
        if (!File.Exists(filePath))
            throw new FileNotFoundException("未找到文件: ", filePath);

        // 打开文件流,准备读取文件内容
        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

        // 使用MD5哈希对象计算文件的哈希值
        byte[] hashBytes = await Task.Run(() => md5.ComputeHash(stream));

        // 将哈希值转换为字符串格式,并返回
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 异步检查指定路径的文件是否为有效的PDF文件.
    /// </summary>
    /// <param name="filePath">文件路径.</param>
    /// <returns>如果文件是有效的PDF文件,则返回true；否则返回false.</returns>
    public static async Task<bool> IsValidPdfFileAsync(string filePath)
    {
        // 检查文件是否存在
        if (!File.Exists(filePath))
            return false;

        // 检查文件扩展名是否为.pdf
        if (!Path.GetExtension(filePath).Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
            return false;

        // 初始化缓冲区用于读取文件头信息
        byte[] buffer = new byte[4];

        // 使用FileStream异步读取文件的前4个字节
        await using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4, true))
        {
            // 如果读取的字节数少于4个,则文件不完整,返回false
            int bytesRead = await stream.ReadAsync(buffer, 0, 4);
            if (bytesRead < 4)
                return false;
        }

        // 检查文件头是否符合PDF文件的格式（%PDF）
        return buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46;
    }

    /// <summary>
    /// 检查指定路径的文件是否为有效的PDF文件.
    /// </summary>
    /// <param name="filePath">文件路径.</param>
    /// <returns>如果文件是有效的PDF文件,则返回true；否则返回false.</returns>
    public static bool IsValidPdfFile(string filePath)
    {
        // 检查文件是否存在
        if (!File.Exists(filePath))
            return false;

        // 检查文件扩展名是否为.pdf
        if (!Path.GetExtension(filePath).Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
            return false;

        // 读取文件开头的4个字节,用于判断是否为PDF文件
        byte[] buffer = new byte[4];
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            // 如果读取的字节数少于4,说明文件不完整
            int bytesRead = stream.Read(buffer, 0, 4);
            if (bytesRead < 4)
                return false;
        }

        // PDF文件的开头四个字节应该是%PDF（十六进制表示为25 50 44 46）
        // 这里检查读取的四个字节是否符合PDF文件的特征
        return buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46;
    }

    /// <summary>
    /// 获取错误占位符图片的字节数组.
    /// </summary>
    /// <returns>占位符图片的字节数组.</returns>
    public static byte[] GetErrorPlaceholderImage()
    {
        // 定义一个内联的Base64编码的占位符图片字符串
        var base64PlaceHolder = """
                                data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII="
                                """;
        // 将Base64编码的字符串转换为字节数组并返回
        return Convert.FromBase64String(base64PlaceHolder);
    }

    /// <summary>
    /// 检查指定路径的图像是否有效.
    /// </summary>
    /// <param name="filePath">图像文件的路径.</param>
    /// <returns>如果图像是有效的,则返回true；否则返回false.</returns>
    public static bool IsValidImage(string filePath)
    {
        // 检查文件是否存在
        if (!File.Exists(filePath))
            return false;

        try
        {
            // 使用Bitmap对象加载图像文件
            using (Bitmap bitmap = new Bitmap(filePath))
            {
                // 锁定图像数据以便直接访问像素
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                // 计算图像数据的总字节数
                int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
                // 创建一个数组来存储像素数据
                byte[] pixelData = new byte[bytes];
                // 将锁定的图像数据复制到byte数组中
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelData, 0, bytes);
                // 解锁图像数据
                bitmap.UnlockBits(bmpData);

                // 遍历像素数据,每4个字节代表一个像素（ARGB格式）
                for (int i = 0; i < pixelData.Length; i += 4)
                {
                    // 检查红色、绿色和蓝色通道是否有非零值
                    if (pixelData[i] != 0 || pixelData[i + 1] != 0 || pixelData[i + 2] != 0)
                    {
                        // 如果找到非零值,说明图像中存在有效像素,返回true
                        return true;
                    }
                }

                // 如果所有像素的RGB值都是零,说明图像是无效(纯黑)的,返回false
                return false;
            }
        }
        catch (Exception)
        {
            // 如果发生异常,说明图像文件有问题,返回false
            return false;
        }
    }
}