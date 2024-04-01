using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class NewPaletteColorWindow : ConfirmationDialog
{
    [Export] private ColorPickerButton voxelColorPicker;
    [Export] private TextEdit minecraftIDEdit;
    [Export] private ConfirmationDialog deleteConfirmationDialog;

    private Button deleteButton;
    private Guid paletteGuid;

    private const string DELETE_ACTION_STRING = "DELETE";

    private bool createSubscribed = false;
    private bool editSubscribed = false;

    private void UnSubcribeConfirmed()
    {
        if (createSubscribed)
        {
            Confirmed -= OnCreate;
            createSubscribed = false;
        }
        else if (editSubscribed)
        {
            Confirmed -= OnSave;
            createSubscribed = false;
        }
    }

    public override void _Ready()
    {
        deleteButton = AddButton("Delete", true, DELETE_ACTION_STRING);
        deleteButton.Hide();
        CustomAction += OnCustomAction;

        deleteConfirmationDialog.Confirmed += OnDelete;
        deleteConfirmationDialog.GetLabel().HorizontalAlignment = HorizontalAlignment.Center;
    }

    private void OnCustomAction(StringName action)
    {
        if (action.ToString() == DELETE_ACTION_STRING)
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
    }

    public void ShowCreateWindow()
    {
        voxelColorPicker.Color = new Color();

        Title = "Create New Palette Color";
        OkButtonText = "Create";

        UnSubcribeConfirmed();
        createSubscribed = true;
        Confirmed += OnCreate;

        deleteButton.Hide();

        Show();
    }

    public void ShowEditWindow(Guid paletteColorGuid)
    {
        VoxelColor voxelColor = GameManager.DataManager.PaletteData.paletteColors[paletteColorGuid];
        voxelColorPicker.Color = voxelColor.color;
        paletteGuid = paletteColorGuid;

        Title = "Edit Palette Color";
        OkButtonText = "Save";

        UnSubcribeConfirmed();
        editSubscribed = true;
        Confirmed += OnSave;

        deleteButton.Show();

        Show();
    }

    private void OnSave()
    {
        VoxelColor voxelColor = GameManager.DataManager.PaletteData.paletteColors[paletteGuid];
        voxelColor.color = voxelColorPicker.Color;

        GameManager.PaletteUI.Update();
        GameManager.SurfaceMesh.UpdateMesh();
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

    private void OnDelete()
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
