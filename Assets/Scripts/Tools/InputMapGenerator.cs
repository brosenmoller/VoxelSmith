using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[Tool]
public partial class InputMapGenerator : EditorScript
{
    private const string outputPath = "res://Assets/Scripts/Tools/InputMapActions.cs";

    public override void _Run()
    {
        UpdateInputMapClass();
    }

    private void UpdateInputMapClass()
    {
        List<StringName> inputMap = InputMap.GetActions().ToList();
        string classContent = GenerateClassContent(inputMap);
        SaveToFile(classContent);
    }

    private string GenerateClassContent(List<StringName> inputMap)
    {
        string className = "InputMapActions";
        string content = $"public static class {className}\n{{\n";

        foreach (StringName action in inputMap)
        {
            string constName = action.ToString().Replace(" ", "");
            content += $"    public const string {constName} = \"{action}\";\n";
        }

        content += "}\n";
        return content;
    }

    private void SaveToFile(string content)
    {
        try
        {
            using (StreamWriter writer = new(outputPath))
            {
                writer.Write(content);
            }
            GD.Print("Input map class generated successfully.");
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error saving file: {e.Message}");
        }
    }
}
