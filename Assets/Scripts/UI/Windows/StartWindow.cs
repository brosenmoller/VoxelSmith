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
        GameManager.UIController.loadProjectDialog.Show();
        Hide();
    }

    private void NewProject()
    {
        GameManager.UIController.newProjectDialog.Show();
        Hide();
    }
}
