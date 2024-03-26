using Godot;
using System.Collections.Generic;

public partial class NewPaletteColorWindow : ConfirmationDialog
{
    [Export] private ColorPickerButton voxelColorPicker;
    [Export] private TextEdit minecraftIDEdit;

    public override void _Ready()
    {
        Confirmed += OnCreate;
    }

    private void OnCreate()
    {
        VoxelColor voxelColor = new() { 
            color = voxelColorPicker.Color, 
            minecraftIDlist = new List<string>() { minecraftIDEdit.Text } 
        };

        GameManager.DataManager.PaletteData.palleteColors.Add(voxelColor);
        GameManager.PaletteUI.Update();
    }
}
