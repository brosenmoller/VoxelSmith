using Godot;
using System;

public abstract partial class PaletteEditWindow : ConfirmationDialog
{
    [Export] private string name;
    [Export] protected ConfirmationDialog deleteConfirmationDialog;
    [Export] protected ColorPickerButton voxelColorPicker;

    private Button deleteButton;
    protected Guid paletteGuid;

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

        OnReady();
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
        OnLoad();

        Title = "Edit Palette " + name;
        OkButtonText = "Save";

        UnSubcribeConfirmed();
        editSubscribed = true;
        Confirmed += OnSave;

        deleteButton.Show();

        Show();
    }

    protected abstract void OnLoad();

    protected abstract void OnDeleteButtonPressed();

    protected abstract void OnSave();

    protected abstract void OnCreate();

    protected abstract void OnDelete();

    protected virtual void OnReady() { }
}

