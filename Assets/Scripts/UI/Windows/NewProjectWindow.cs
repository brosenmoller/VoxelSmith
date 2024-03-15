using Godot;
using System;

public partial class NewProjectWindow : ConfirmationDialog
{
    [Export] private TextEdit projectName;
    [Export] private TextEdit saveDirectoryPath;
    [Export] private TextEdit palettePath;
    [Export] private Button openProjectDirectoryButton;
    [Export] private Button openPaletteButton;
    [Export] private Button newPaletteButton;
    [Export] private FileDialog projectDirectoryFileDialog;

    public override void _Ready()
    {
        openProjectDirectoryButton.Pressed += projectDirectoryFileDialog.Show;
        projectDirectoryFileDialog.Confirmed += OnDirectorySelected;

        Confirmed += OnNewProjectConfirmed;
    }

    private void OnNewProjectConfirmed()
    {
        if (projectName.Text.Length < 0 || saveDirectoryPath.Text.Length < 0)
        {
            return;
        }

        GameManager.DataManager.CreateNewProject(Name, saveDirectoryPath.Text + "\\", Guid.NewGuid());
        Hide();
    }

    private void OnDirectorySelected()
    {
        saveDirectoryPath.Text = projectDirectoryFileDialog.CurrentDir;
    }
}
