using Godot;

public partial class StartWindow : Window
{
    [Export] private Button loadProjectButton;
    [Export] private Button newProjectButton;

    public override void _Ready()
    {
        loadProjectButton.Pressed += LoadProject;
        newProjectButton.Pressed += NewProject;
        DataManager.OnProjectLoad += HandleProjectLoad;
    }

    private void HandleProjectLoad()
    {
        Hide();
        GameManager.WorldController.WorldInFocus = true;
        GameManager.UIController.ClickBlockerLayer.Visible = false;
    }

    private void LoadProject()
    {
        GameManager.UIController.ShowLoadProjectDialog();
    }

    private void NewProject()
    {
        GameManager.UIController.newProjectDialog.Show();
    }
}
