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
        if (GameManager.DataManager.EditorData.importPaths.ContainsKey(GameManager.DataManager.ProjectData.id))
        {
            EditorData.ImportPathData importSettings = GameManager.DataManager.EditorData.importPaths[GameManager.DataManager.ProjectData.id];
            GameManager.UIController.ImportPath(importSettings.path);
        }
    }
}

