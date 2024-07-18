using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class ToolOptionRangeAttribute : Attribute
{
    public float Min { get; }
    public float Max { get; }

    public ToolOptionRangeAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}