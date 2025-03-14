using System.ComponentModel;
using System.Reflection;

namespace SkillLevelEvaluationExporter.Utils;

public class ReflectionUtil
{
    public static string GetEnumDescription(Enum value)
    {
        try
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
        catch (NullReferenceException)
        {
            return value.ToString();
        }
    }
}