using System.IO;
using Godot;

public partial class NewProjectWindow : ConfirmationDialog
{
    [Export] private TextEdit projectName;
    [Export] private TextEdit saveDirectoryPath;
    [Export] private Button openProjectDirectoryButton;
    [Export] private OptionButton paletteOptionButton;

    [ExportSubgroup("Unused")]
    [Export] private Button openPaletteButton;
    [Export] private Button newPaletteButton;

    public override void _Ready()
    {
        Confirmed += HandleNewProjectConfirmed;

        openProjectDirectoryButton.Pressed += HandleOpenDirectoryPressed;
        VisibilityChanged += HandleVisibilityChanged;

        openPaletteButton.Pressed += GameManager.UIController.ShowLoadPaletteDialog;
        newPaletteButton.Pressed += GameManager.UIController.ShowCreateNewPaletteDialog;

        SetupPaletteOptionButton();
    }

    private void HandleVisibilityChanged()
    {
        if (!Visible) 
        {
            if (GameManager.IsInitialized)
            {
                GameManager.UIController.ClickBlockerLayer.Visible = false;
            }
            return;
        }

        GameManager.UIController.ClickBlockerLayer.Visible = true;
        projectName.Text = string.Empty;
    }

    private void HandleOpenDirectoryPressed()
    {
        string directory = GameManager.IsInitialized ? Path.GetDirectoryName(GameManager.DataManager.EditorData.savePaths[GameManager.DataManager.ProjectData.id]) : string.Empty;
        GameManager.NativeDialog.ShowFileDialog("Select Project Directory", DisplayServer.FileDialogMode.OpenDir, [], (NativeDialog.Info info) =>
        {
            HandleDirectorySelected(info.path);
        }, directory);
    }

    private void SetupPaletteOptionButton()
    {
        paletteOptionButton.AddItem("Default", (int)PaletteOption.Default);
        paletteOptionButton.AddItem("Blank", (int)PaletteOption.Blank);
    }

    private void HandleNewProjectConfirmed()
    {
        if (projectName.Text.Length <= 0 || saveDirectoryPath.Text.Length <= 0)
        {
            // TODO: Show Error
            return;
        }

        GameManager.DataManager.CreateNewProject(projectName.Text, saveDirectoryPath.Text, (PaletteOption)paletteOptionButton.Selected);
        Hide();
    }

    private void HandleDirectorySelected(string path)
    {
        saveDirectoryPath.Text = path;
    }

    public enum PaletteOption
    {
        Default = 0,
        Blank = 1,
    }
}