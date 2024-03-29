using Godot;
using System;
using System.IO;

public partial class NewPalettePrefabWindow : ConfirmationDialog
{
    [Export] private ColorPickerButton voxelColorPicker;
    [Export] private Button loadPrefabButton;
    [Export] private FileDialog loadPrefabFileDialog;

    [Export] private TextEdit prefabNameTextEdit;

    [Export] private TextEdit godotSceneIdTextEdit;
    [Export] private TextEdit unityPrefabGuidTextEdit;
    [Export] private TextEdit unityPrefabTranformFileIdTextEdit;

    public override void _Ready()
    {
        Confirmed += OnCreate;
        loadPrefabButton.Pressed += loadPrefabFileDialog.Show;
        loadPrefabFileDialog.Confirmed += LoadPrefab;
    }

    private void LoadPrefab()
    {
        if (loadPrefabFileDialog.CurrentFile.Length <= 0 && loadPrefabFileDialog.CurrentDir.Length <= 0) 
        { 
            return; 
        }

        string path = Path.Combine(loadPrefabFileDialog.CurrentDir, loadPrefabFileDialog.CurrentFile);

        GD.Print("Not implemented, but here's the path: " + path);
    }

    private void OnCreate()
    {
        VoxelPrefab voxelPrefab = new()
        {
            color = voxelColorPicker.Color,
            prefabName = prefabNameTextEdit.Text,
            unityPrefabTransformFileId = unityPrefabTranformFileIdTextEdit.Text,
            godotSceneID = godotSceneIdTextEdit.Text,
            unityPrefabGuid = unityPrefabGuidTextEdit.Text,
        };

        GameManager.DataManager.PaletteData.palletePrefabs.Add(Guid.NewGuid(), voxelPrefab);
        GameManager.PaletteUI.Update();
    }
}

