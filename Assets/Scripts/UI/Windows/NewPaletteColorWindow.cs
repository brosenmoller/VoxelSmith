using Godot;
using System;
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
            minecraftIDlist = new List<string>() { "minecraft:" + minecraftIDEdit.Text } 
        };

        GameManager.DataManager.PaletteData.paletteColors.Add(Guid.NewGuid(), voxelColor);
        GameManager.PaletteUI.Update();
    }
}
