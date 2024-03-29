using Godot;

public partial class NewProjectWindow : ConfirmationDialog
{
    [Export] private TextEdit projectName;
    [Export] private TextEdit saveDirectoryPath;
    [Export] private Button openProjectDirectoryButton;
    [Export] private FileDialog projectDirectoryFileDialog;
    [Export] private OptionButton paletteOptionButton;

    [ExportSubgroup("Unused")]
    [Export] private Button openPaletteButton;
    [Export] private Button newPaletteButton;

    public override void _Ready()
    {
        projectDirectoryFileDialog.Confirmed += OnDirectorySelected;
        Confirmed += OnNewProjectConfirmed;

        openProjectDirectoryButton.Pressed += projectDirectoryFileDialog.Show;

        openPaletteButton.Pressed += GameManager.UIController.loadPaletteDialog.Show;
        newPaletteButton.Pressed += GameManager.UIController.newPaletteFileDialog.Show;

        SetupPaletteOptionButton();
    }

    private void SetupPaletteOptionButton()
    {
        paletteOptionButton.AddItem("Default", (int)PaletteOption.Default);
        paletteOptionButton.AddItem("Blank", (int)PaletteOption.Blank);
    }

    private void OnNewProjectConfirmed()
    {
        if (projectName.Text.Length <= 0 || saveDirectoryPath.Text.Length <= 0)
        {
            // TODO: Show Error

            return;
        }

        GameManager.DataManager.CreateNewProject(projectName.Text, saveDirectoryPath.Text, (PaletteOption)paletteOptionButton.Selected);
        Hide();
    }

    private void OnDirectorySelected()
    {
        saveDirectoryPath.Text = projectDirectoryFileDialog.CurrentDir;
    }

    public enum PaletteOption
    {
        Default = 0,
        Blank = 1,
    }
}
