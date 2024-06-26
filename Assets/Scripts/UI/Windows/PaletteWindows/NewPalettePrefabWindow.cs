using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public partial class NewPalettePrefabWindow : PaletteEditWindow
{
    [Export] private Button loadPrefabButton;

    [Export] private TextEdit prefabNameTextEdit;

    [Export] private TextEdit godotSceneIdTextEdit;
    [Export] private TextEdit unityPrefabGuidTextEdit;
    [Export] private TextEdit unityPrefabTranformFileIdTextEdit;

    protected override void OnReady()
    {
        loadPrefabButton.Pressed += OnButtonPress;

        prefabNameTextEdit.RemoveChild(prefabNameTextEdit.GetVScrollBar());
        godotSceneIdTextEdit.RemoveChild(godotSceneIdTextEdit.GetVScrollBar());
        unityPrefabGuidTextEdit.RemoveChild(unityPrefabGuidTextEdit.GetVScrollBar());
        unityPrefabTranformFileIdTextEdit.RemoveChild(unityPrefabTranformFileIdTextEdit.GetVScrollBar());
    }

    private void OnButtonPress()
    {
        GameManager.NativeDialog.ShowFileDialog("Open a Prefab File", DisplayServer.FileDialogMode.OpenFile, new string[] { "*.prefab", "*.tscn" }, (NativeDialog.Info info) =>
        {
            LoadPrefab(info.directory, info.fileName);
        });
    }

    private void LoadPrefab(string dir, string file)
    {
        if (dir.Length <= 0 || file.Length <= 0) 
        { 
            return; 
        }

        string prefabPath = Path.Combine(dir, file);
        string prefabMetaPath = prefabPath + ".meta";

        if (!File.Exists(prefabPath) || !File.Exists(prefabMetaPath))
        {
            // TODO : Show Error to the User
            GD.PrintErr("Couldn't find files");
            return;
        }

        string prefabFileString = File.ReadAllText(prefabPath);
        string prefabMetaFileString = File.ReadAllText(prefabMetaPath);

        string guidRegex = @"[0-9a-fA-F]{32}";
        Match guidMatch = Regex.Match(prefabMetaFileString, guidRegex);
        if (guidMatch.Success) 
        { 
            unityPrefabGuidTextEdit.Text = guidMatch.Value;
        }

        // Making sure it is the root gameobject
        string fatherLess = "m_Father: {fileID: 0}";
        int indexFatherLess = prefabFileString.IndexOf(fatherLess);

        string transformIDRegex = @"(?<=---\s\!u\!4\s\&)\s*(-?\d+)";

        Regex regex = new(transformIDRegex, RegexOptions.Compiled);
        MatchCollection matches = regex.Matches(prefabFileString);

        int closestDistance = int.MaxValue;
        foreach (Match match in matches)
        {
            int distance = Mathf.Abs(match.Index - indexFatherLess);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                unityPrefabTranformFileIdTextEdit.Text = match.Value;
            }
        }
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

        if (voxelPrefab.color != voxelColorPicker.Color)
        {
            GameManager.PrefabMesh.UpdateAllOfGUID(paletteGuid);
        }

        voxelPrefab.color = voxelColorPicker.Color;

        voxelPrefab.minecraftIDlist = GetCompeletedMinecraftID();

        voxelPrefab.prefabName = prefabNameTextEdit.Text;
        voxelPrefab.unityPrefabGuid = unityPrefabGuidTextEdit.Text;
        voxelPrefab.unityPrefabTransformFileId = unityPrefabTranformFileIdTextEdit.Text;
        voxelPrefab.godotSceneID = godotSceneIdTextEdit.Text;

        GameManager.PaletteUI.Update();
    }

    protected override void OnCreate()
    {
        Guid newID = Guid.NewGuid();
        VoxelPrefab voxelPrefab = new()
        {
            id = newID,
            color = voxelColorPicker.Color,
            minecraftIDlist = GetCompeletedMinecraftID(),
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
        if (!GameManager.DataManager.PaletteData.palletePrefabs.ContainsKey(paletteGuid))
        {
            return; // TODO: Error
        }

        GameManager.DataManager.PaletteData.palletePrefabs.Remove(paletteGuid);

        List<Vector3I> keyList = GameManager.DataManager.ProjectData.voxelPrefabs.Keys.ToList();
        for (int i = keyList.Count - 1; i >= 0; i--)
        {
            if (GameManager.DataManager.ProjectData.voxelPrefabs[keyList[i]] == paletteGuid)
            {
                GameManager.PrefabMesh.ClearVoxel(keyList[i]);
            }
        }

        GameManager.PaletteUI.Update();
        Hide();
    }

    protected override void OnLoad()
    {
        VoxelPrefab voxelPrefab = GameManager.DataManager.PaletteData.palletePrefabs[paletteGuid];

        prefabNameTextEdit.Text = voxelPrefab.prefabName;
        unityPrefabGuidTextEdit.Text = voxelPrefab.unityPrefabGuid;
        unityPrefabTranformFileIdTextEdit.Text = voxelPrefab.unityPrefabTransformFileId;
        godotSceneIdTextEdit.Text = voxelPrefab.godotSceneID;
    }
}

