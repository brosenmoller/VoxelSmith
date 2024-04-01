using Godot;
using System;

public abstract partial class PaletteEditWindow : ConfirmationDialog
{
    [Export] private string name;
    [Export] private ConfirmationDialog deleteConfirmationDialog;
    [Export] private ColorPickerButton voxelColorPicker;

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
        if (action == DELETE_ACTION_STRING)
        {
            OnDeleteButtonPressed();
        }
    }

    protected abstract void OnDeleteButtonPressed();

    public void ShowCreateWindow()
    {
        voxelColorPicker.Color = Color.Color8(255, 255, 255);

        Title = "Create New Palette " + name;
        OkButtonText = "Create";

        UnSubcribeConfirmed();
        createSubscribed = true;
        Confirmed += OnCreate;

        deleteButton.Hide();

        Show();
    }

    public void ShowEditWindow(Guid paletteColorGuid)
    {
        VoxelData voxelData = null;
        if (GameManager.DataManager.PaletteData.paletteColors.ContainsKey(paletteColorGuid))
        {
            voxelData = GameManager.DataManager.PaletteData.paletteColors[paletteColorGuid];
        }
        else if (GameManager.DataManager.PaletteData.palletePrefabs.ContainsKey(paletteColorGuid))
        {
            voxelData = GameManager.DataManager.PaletteData.palletePrefabs[paletteColorGuid];
        }

        if (voxelData != null)
        {
            voxelColorPicker.Color = voxelData.color;
            paletteGuid = paletteColorGuid;
        }
        else
        {
            return;
        }

        Title = "Edit Palette " + name;
        OkButtonText = "Save";

        UnSubcribeConfirmed();
        editSubscribed = true;
        Confirmed += OnSave;

        deleteButton.Show();

        Show();
    }

    protected abstract void OnSave();

    protected abstract void OnCreate();

    protected abstract void OnDelete()
}

