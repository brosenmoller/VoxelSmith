using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class NewPalettePrefabWindow : PaletteEditWindow
{
    [Export] private Button loadPrefabButton;
    [Export] private FileDialog loadPrefabFileDialog;

    [Export] private TextEdit prefabNameTextEdit;

    [Export] private TextEdit godotSceneIdTextEdit;
    [Export] private TextEdit unityPrefabGuidTextEdit;
    [Export] private TextEdit unityPrefabTranformFileIdTextEdit;

    protected override void OnReady()
    {
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


    protected override void OnDeleteButtonPressed()
    {
        if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsValue(paletteGuid))
        {
            deleteConfirmationDialog.Show();
        }
        else
        {
            OnDelete();
        }
    }

    protected override void OnSave()
    {
        VoxelPrefab voxelPrefab = GameManager.DataManager.PaletteData.palletePrefabs[paletteGuid];
        voxelPrefab.color = voxelColorPicker.Color;

        GameManager.PaletteUI.Update();
        GameManager.SurfaceMesh.UpdateMesh();
    }

    protected override void OnCreate()
    {
        Guid newID = Guid.NewGuid();
        VoxelPrefab voxelPrefab = new()
        {
            id = newID,
            color = voxelColorPicker.Color,
            prefabName = prefabNameTextEdit.Text,
            unityPrefabTransformFileId = unityPrefabTranformFileIdTextEdit.Text,
            godotSceneID = godotSceneIdTextEdit.Text,
            unityPrefabGuid = unityPrefabGuidTextEdit.Text,
        };

       
        GameManager.DataManager.PaletteData.palletePrefabs.Add(newID, voxelPrefab);
        GameManager.PaletteUI.Update();
    }


    protected override void OnDelete()
    {
        if (GameManager.DataManager.PaletteData.palletePrefabs.ContainsKey(paletteGuid))
        {
            GameManager.DataManager.PaletteData.palletePrefabs.Remove(paletteGuid);

            List<Vector3I> keyList = GameManager.DataManager.ProjectData.voxelPrefabs.Keys.ToList();
            for (int i = keyList.Count - 1; i >= 0; i--)
            {
                if (GameManager.DataManager.ProjectData.voxelPrefabs[keyList[i]] == paletteGuid)
                {
                    GameManager.DataManager.ProjectData.voxelPrefabs.Remove(keyList[i]);
                }
            }

            GameManager.PaletteUI.Update();
            GameManager.PrefabMesh.UpdateMesh();
            Hide();
        }
        else
        {
            // TODO: Error
        }
    }

    protected override void OnLoad()
    {
        VoxelPrefab voxelPrefab = GameManager.DataManager.PaletteData.palletePrefabs[paletteGuid];
        voxelColorPicker.Color = voxelPrefab.color;
    }
}

