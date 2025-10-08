using Godot;

public partial class QuickImport : MarginContainer
{
    private Button button;

    public override void _Ready()
    {
        button = this.GetChildByType<Button>();
        button.Pressed += OnButtonPress;
    }

    private void OnButtonPress()
    {
        if (!GameManager.IsInitialized) { return; }

        if (GameManager.DataManager.EditorData.importPaths.TryGetValue(GameManager.DataManager.ProjectData.id, out EditorData.ImportPathData importSettings))
        {
            GameManager.UIController.ImportPath(importSettings.path);
        }
    }
}

