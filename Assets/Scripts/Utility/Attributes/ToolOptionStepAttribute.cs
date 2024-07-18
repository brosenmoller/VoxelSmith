using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class ToolOptionStepAttribute : Attribute
{
    public float Step { get; }

    public ToolOptionStepAttribute(float step)
    {
        Step = step;
    }
}