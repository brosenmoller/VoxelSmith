using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class NewPaletteColorWindow : PaletteEditWindow
{
    protected override void OnDeleteButtonPressed()
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsValue(paletteGuid))
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
        VoxelColor voxelColor = GameManager.DataManager.PaletteData.paletteColors[paletteGuid];
        voxelColor.color = voxelColorPicker.Color;

        GameManager.PaletteUI.Update();
        GameManager.SurfaceMesh.UpdateMesh();
    }

    protected override void OnCreate()
    {
        Guid newID = Guid.NewGuid();
        VoxelColor voxelColor = new()
        {
            id = newID,
            color = voxelColorPicker.Color,
            minecraftIDlist = GetCompeletedMinecraftID(),
        };

        GameManager.DataManager.PaletteData.paletteColors.Add(newID, voxelColor);
        GameManager.PaletteUI.Update();
    }

    protected override void OnDelete()
    {
        if (GameManager.DataManager.PaletteData.paletteColors.ContainsKey(paletteGuid))
        {
            GameManager.DataManager.PaletteData.paletteColors.Remove(paletteGuid);

            List<Vector3I> keyList = GameManager.DataManager.ProjectData.voxelColors.Keys.ToList();
            for (int i = keyList.Count - 1; i >= 0; i--)
            {
                if (GameManager.DataManager.ProjectData.voxelColors[keyList[i]] == paletteGuid)
                {
                    GameManager.DataManager.ProjectData.voxelColors.Remove(keyList[i]);
                }
            }

            GameManager.PaletteUI.Update();
            GameManager.SurfaceMesh.UpdateMesh();
            Hide();
        }
        else
        {
            // TODO: Error
        }
    }
}