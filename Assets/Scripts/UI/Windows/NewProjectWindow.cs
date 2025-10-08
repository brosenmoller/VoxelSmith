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
        Confirmed += OnNewProjectConfirmed;

        openProjectDirectoryButton.Pressed += OnButtonPress;
        VisibilityChanged += HandleVisibilityChanged;

        openPaletteButton.Pressed += GameManager.UIController.ShowLoadPaletteDialog;
        newPaletteButton.Pressed += GameManager.UIController.ShowCreateNewPaletteDialog;

        SetupPaletteOptionButton();
    }

    private void HandleVisibilityChanged()
    {
        if (!Visible) 
        {
            GameManager.UIController.ClickBlockerLayer.Visible = false;
            return; 
        }
        GameManager.UIController.ClickBlockerLayer.Visible = true;

        projectName.Text = string.Empty;
    }

    private void OnButtonPress()
    {
        GameManager.NativeDialog.ShowFileDialog("Select Project Directory", DisplayServer.FileDialogMode.OpenDir, System.Array.Empty<string>(), (NativeDialog.Info info) =>
        {
            OnDirectorySelected(info.path);
        });
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

    private void OnDirectorySelected(string path)
    {
        saveDirectoryPath.Text = path;
    }

    public enum PaletteOption
    {
        Default = 0,
        Blank = 1,
    }
}