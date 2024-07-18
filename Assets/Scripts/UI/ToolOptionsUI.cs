using Godot;
using System;
using System.Reflection;

public partial class ToolOptionsUI : Control
{
    [Export] private Control toolOptionsParent;

    public void ClearToolOptions()
    {
        toolOptionsParent.RemoveAllChildren();
    }
    
    public void SetToolOptions(IToolOptions toolOptions)
    {
        toolOptionsParent.RemoveAllChildren();

        foreach (FieldInfo fieldInfo in toolOptions.GetType().GetFields()) 
        {
            AddOption(toolOptions, fieldInfo);
        }
    }

    private void AddOption(IToolOptions toolOptions, FieldInfo fieldInfo)
    {
        HBoxContainer parent = new();

        MarginContainer marginContainer = new() { CustomMinimumSize = new Vector2(150, 0) };
        marginContainer.AddThemeConstantOverride("margin_top", 3);

        RichTextLabel label = new() { Text = fieldInfo.Name, CustomMinimumSize = new Vector2(0, 36) };

        Control typeSpecificUI = fieldInfo.GetValue(toolOptions) switch
        {
            int _ => AddOption_Number(toolOptions, fieldInfo),
            float _ => AddOption_Number(toolOptions, fieldInfo, 0.01f),
            double _ => AddOption_Number(toolOptions, fieldInfo, 0.01f),
            bool _ => AddOption_Bool(toolOptions, fieldInfo),
            string _ => AddOption_String(toolOptions, fieldInfo),
            _ => throw new Exception($"Unsupported Type in ToolOptionsUI\n name: {fieldInfo.Name} type: {fieldInfo.GetType()}"),
        };

        toolOptionsParent.AddChild(parent);
        parent.AddChild(marginContainer);
        marginContainer.AddChild(label);

        parent.AddChild(typeSpecificUI);
    }


    private Control AddOption_Number(IToolOptions toolOptions, FieldInfo fieldInfo, float defaultStep = 1f, float defaultMin = 0f, float defaultMax = 100f)
    {
        float step = defaultStep, min = defaultMin, max = defaultMax;
        bool hasSlider = false;

        if (fieldInfo.GetAttribute(out ToolOptionStepAttribute stepAttribute)) { step = stepAttribute.Step; }
        if (fieldInfo.GetAttribute(out ToolOptionRangeAttribute rangeAttribute)) { min = rangeAttribute.Min; max = rangeAttribute.Max; }
        if (fieldInfo.HasAttribute<ToolOptionSliderAttribute>()) { hasSlider = true; }

        double value = 0;
        Type fieldType = fieldInfo.FieldType;
        if (fieldType == typeof(int)) { value = (int)fieldInfo.GetValue(toolOptions); }
        else if (fieldType == typeof(float)) { value = (float)fieldInfo.GetValue(toolOptions); }
        else if (fieldType == typeof(double)) { value = (double)fieldInfo.GetValue(toolOptions); }

        Godot.Range range;
        if (hasSlider)
        {
            range = new HSlider() { Value = value, Step = step, MinValue = min, MaxValue = max };
        }
        else
        {
            range = new SpinBox() { Value = value, Step = step, MinValue = min, MaxValue = max };
        }

        if (fieldType == typeof(int)) 
        {
            range.ValueChanged += (double value) => fieldInfo.SetValue(toolOptions, (int)value);
        }
        else if (fieldType == typeof(float))
        {
            range.ValueChanged += (double value) => fieldInfo.SetValue(toolOptions, (float)value);
        }
        else if (fieldType == typeof(double))
        {
            range.ValueChanged += (double value) => fieldInfo.SetValue(toolOptions, (double)value);
        }
        
        return range;
    }

    private Control AddOption_Bool(IToolOptions toolOptions, FieldInfo fieldInfo)
    {
        CheckBox checkBox = new() { ButtonPressed = (bool)fieldInfo.GetValue(toolOptions), ToggleMode = true };
        checkBox.Toggled += (bool value) => fieldInfo.SetValue(toolOptions, value);
        return checkBox;
    }

    private Control AddOption_String(IToolOptions toolOptions, FieldInfo fieldInfo)
    {
        LineEdit lineEdit = new() { Text = (string)fieldInfo.GetValue(toolOptions) };
        lineEdit.TextChanged += (string value) => fieldInfo.SetValue(toolOptions, value);
        return lineEdit;
    }
}

public interface IToolOptions { }
