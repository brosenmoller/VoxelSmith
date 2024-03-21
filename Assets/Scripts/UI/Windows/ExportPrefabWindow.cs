using Godot;

public partial class ExportPrefabWindow : FileDialog
{
    public override void _Ready()
    {
        Confirmed += OnConfirmed;
    }

    private void OnConfirmed()
    {
        if (CurrentFile.Length <= 0 && CurrentDir.Length <= 0) { return; }

        GameManager.ExportManager.ExportUnityPrefab(CurrentDir, CurrentFile);
    }
}
