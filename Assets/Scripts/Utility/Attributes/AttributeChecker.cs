using System;
using System.Reflection;

public static class AttributeChecker
{
    public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
    {
        return Attribute.IsDefined(memberInfo, typeof(T));
    }

    public static bool GetAttribute<T>(this MemberInfo memberInfo, out T value) where T : Attribute
    {
        if (memberInfo.HasAttribute<T>())
        {
            value = (T)Attribute.GetCustomAttribute(memberInfo, typeof(T));
            return true;
        }

        value = default;
        return false;
    }
}
