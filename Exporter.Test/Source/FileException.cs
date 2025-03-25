namespace Exporter.Test.Source;

public class QuestionIdentifier
{
    public required int Major { get; init; }
    public required int Minor { get; init; }
    public required int Build { get; init; }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Build}";
    }

    public static QuestionIdentifier? Parse(string str)
    {
        var parts = str.Split('.');
        if (parts.Length != 3)
        {
            Console.WriteLine("错误的输入格式!");
            return null;
        }

        int major, minor, build;
        if (!int.TryParse(parts[0], out major) || !int.TryParse(parts[1], out minor) || !int.TryParse(parts[2], out build))
        {
            Console.WriteLine("错误的输入格式!");
            return null;
        }

        return new QuestionIdentifier { Major = major, Minor = minor, Build = build };
    }

    public override bool Equals(object? obj)
    {
        return obj is QuestionIdentifier identifier &&
               Major == identifier.Major &&
               Minor == identifier.Minor &&
               Build == identifier.Build;
    }

    protected bool Equals(QuestionIdentifier other)
    {
        return Major == other.Major && Minor == other.Minor && Build == other.Build;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Build);
    }
}

public class FileException
{
    public required List<string> FileExceptionList { get; set; }

    public required Dictionary<string, List<QuestionIdentifier>> QuestionExceptionDictionary { get; set; }
}