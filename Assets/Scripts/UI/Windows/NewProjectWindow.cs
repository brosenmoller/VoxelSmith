using Godot;
using System;

public partial class NewProjectWindow : ConfirmationDialog
{
    [Export] private TextEdit projectName;
    [Export] private TextEdit saveDirectoryPath;
    [Export] private Button openProjectDirectoryButton;
    [Export] private Button openPaletteButton;
    [Export] private Button newPaletteButton;
    [Export] private FileDialog projectDirectoryFileDialog;

    public override void _Ready()
    {
        projectDirectoryFileDialog.Confirmed += OnDirectorySelected;
        Confirmed += OnNewProjectConfirmed;

        openProjectDirectoryButton.Pressed += projectDirectoryFileDialog.Show;

        openPaletteButton.Pressed += GameManager.UIController.loadPaletteDialog.Show;
        newPaletteButton.Pressed += GameManager.UIController.newPaletteFileDialog.Show;
    }

    private void OnNewProjectConfirmed()
    {
        if (projectName.Text.Length <= 0 || saveDirectoryPath.Text.Length <= 0)
        {
            // TODO: Show Error

            return;
        }

        GameManager.DataManager.CreateNewProject(projectName.Text, saveDirectoryPath.Text, Guid.NewGuid());
        Hide();
    }

    private void OnDirectorySelected()
    {
        saveDirectoryPath.Text = projectDirectoryFileDialog.CurrentDir;
    }
}
