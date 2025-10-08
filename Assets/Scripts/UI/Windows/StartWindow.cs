using Godot;

public partial class StartWindow : Window
{
    [Export] private Button loadProjectButton;
    [Export] private Button newProjectButton;

    public override void _Ready()
    {
        loadProjectButton.Pressed += LoadProject;
        newProjectButton.Pressed += NewProject;
    }

    private void LoadProject()
    {
        GameManager.UIController.ShowLoadProjectDialog();
        GameManager.UIController.ClickBlockerLayer.Visible = false;
        GameManager.WorldController.WorldInFocus = true;
        Hide();
    }

    private void NewProject()
    {
        GameManager.UIController.newProjectDialog.Show();
        GameManager.UIController.ClickBlockerLayer.Visible = false;
        GameManager.WorldController.WorldInFocus = true;
        Hide();
    }
}
