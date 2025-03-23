namespace Exporter.Test.Source;

public static class FileSource
{
    public static IEnumerable<string[]> GetFiles()
    {
        yield return ["Resources/PDF/信息运维检修工-2024年05月17日机考题库.pdf", "信息运维检修工-2024年05月17日机考题库"];
        yield return ["Resources/PDF/变配电运行值班员（500kV及以上）-2024年05月17日机考题库.pdf", "变配电运行值班员（500kV及以上）-2024年05月17日机考题库"];
        yield return ["Resources/PDF/土建施工员-2024年06月28日机考题库.pdf", "土建施工员-2024年06月28日机考题库"];
        yield return ["Resources/PDF/电网调度自动化厂站端调试检修工-2024年05月16日机考题库.pdf", "电网调度自动化厂站端调试检修工-2024年05月16日机考题库"];
        yield return ["Resources/PDF/装表接电工-2025年03月11日机考题库.pdf", "装表接电工-2025年03月11日机考题库"];
    }
}