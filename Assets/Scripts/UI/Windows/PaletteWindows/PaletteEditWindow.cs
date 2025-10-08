using Godot;
using System;
using System.Collections.Generic;

public abstract partial class PaletteEditWindow : ConfirmationDialog
{
    [Export] private string name;
    [Export] protected ConfirmationDialog deleteConfirmationDialog;
    [Export] protected ColorPickerButton voxelColorPicker;
    [Export] protected TextEdit minecraftIDEdit;

    private Button deleteButton;
    protected Guid paletteGuid;

    private const string DELETE_ACTION_STRING = "DELETE";

    private bool createSubscribed = false;
    private bool editSubscribed = false;

    protected List<string> GetCompeletedMinecraftID() => new() { "minecraft:" + minecraftIDEdit.Text };

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

        minecraftIDEdit.RemoveChild(minecraftIDEdit.GetVScrollBar());

        VisibilityChanged += HandleVisibilityChanged;

        OnReady();
    }

    private void HandleVisibilityChanged()
    {
        if (!Visible)
        {
            GameManager.UIController.ClickBlockerLayer.Visible = false;
            return;
        }
        GameManager.UIController.ClickBlockerLayer.Visible = true;
    }

    private void OnCustomAction(StringName action)
    {
        if (action == DELETE_ACTION_STRING)
        {
            OnDeleteButtonPressed();
        }
    }

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

    public void ShowEditWindow(Guid paletteGuid)
    {
        this.paletteGuid = paletteGuid;
        VoxelData voxelData;

        if (GameManager.DataManager.PaletteData.paletteColors.ContainsKey(paletteGuid)) 
        { 
            voxelData = GameManager.DataManager.PaletteData.paletteColors[paletteGuid]; 
        }
        else 
        { 
            voxelData = GameManager.DataManager.PaletteData.palletePrefabs[paletteGuid]; 
        }

        voxelColorPicker.Color = voxelData.color;
        if (voxelData.minecraftIDlist[0].Length > 10)
        {
            minecraftIDEdit.Text = voxelData.minecraftIDlist[0][10..];
        }
        else
        {
            minecraftIDEdit.Text = "";
        }
        

        OnLoad();

        Title = "Edit Palette " + name;
        OkButtonText = "Save";

        UnSubcribeConfirmed();
        editSubscribed = true;
        Confirmed += OnSave;

        deleteButton.Show();

        Show();
    }

    protected virtual void OnLoad() { }

    protected abstract void OnDeleteButtonPressed();

    protected abstract void OnSave();

    protected abstract void OnCreate();

    protected abstract void OnDelete();

    protected virtual void OnReady() { }
}

