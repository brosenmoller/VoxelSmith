using Godot;
using System.Reflection;

public partial class ToolOptionsUI : Control
{
    [Export] private Control toolOptionsParent;
    
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
            int _ => AddOption_Int(toolOptions, fieldInfo),
            float _ => AddOption_Float(toolOptions, fieldInfo),
            double _ => AddOption_Double(toolOptions, fieldInfo),
            bool _ => AddOption_Bool(toolOptions, fieldInfo),
            string _ => AddOption_String(toolOptions, fieldInfo),
            _ => throw new System.Exception($"Unsupported Type in ToolOptionsUI\n name: {fieldInfo.Name} type: {fieldInfo.GetType()}"),
        };

        toolOptionsParent.AddChild(parent);
        parent.AddChild(marginContainer);
        marginContainer.AddChild(label);

        parent.AddChild(typeSpecificUI);
    }

    private Control AddOption_Int(IToolOptions toolOptions, FieldInfo fieldInfo)
    {
        SpinBox spinBox = new() { Value = (int)fieldInfo.GetValue(toolOptions), Step = 1};
        spinBox.ValueChanged += (double value) => fieldInfo.SetValue(toolOptions, (int)value);
        return spinBox;
    }

    private Control AddOption_Float(IToolOptions toolOptions, FieldInfo fieldInfo)
    {
        SpinBox spinBox = new() { Value = (float)fieldInfo.GetValue(toolOptions), Step = 0.01f };
        spinBox.ValueChanged += (double value) => fieldInfo.SetValue(toolOptions, (float)value);
        return spinBox;
    }

    private Control AddOption_Double(IToolOptions toolOptions, FieldInfo fieldInfo)
    {
        SpinBox spinBox = new() { Value = (double)fieldInfo.GetValue(toolOptions), Step = 0.01 };
        spinBox.ValueChanged += (double value) => fieldInfo.SetValue(toolOptions, value);
        return spinBox;
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
