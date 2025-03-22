using System.ComponentModel;
using System.Reflection;

// 定义一个工具类,用于反射操作
namespace SkillLevelEvaluationExporter.Utils;

public static class ReflectionUtil
{
    /// <summary>
    /// 获取枚举值的描述属性
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>枚举值的描述属性,如果没有描述属性则返回枚举值的字符串表示</returns>
    public static string GetEnumDescription(Enum value)
    {
        try
        {
            // 获取枚举值对应的字段信息
            FieldInfo? field = value.GetType().GetField(value.ToString());
            // 如果字段信息为空,则直接返回枚举值的字符串表示
            if (field == null)
            {
                return value.ToString();
            }

            // 获取字段的DescriptionAttribute属性
            var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            // 将属性转换为DescriptionAttribute类型
            DescriptionAttribute? attribute = attr as DescriptionAttribute;
            // 如果描述属性为空,则返回枚举值的字符串表示；否则返回描述属性的值
            return attribute == null ? value.ToString() : attribute.Description;
        }
        // 捕获空引用异常,并返回枚举值的字符串表示
        catch (NullReferenceException)
        {
            return value.ToString();
        }
    }
}