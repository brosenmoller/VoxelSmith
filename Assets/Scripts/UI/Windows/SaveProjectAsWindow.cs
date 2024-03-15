using Godot;
using System.IO;

public partial class SaveProjectAsWindow : FileDialog
{
    public override void _Ready()
    {
        Confirmed += OnConfirmed;
    }

    private void OnConfirmed()
    {
        if (CurrentFile.Length <= 0 && CurrentDir.Length <= 0) { return; }

        string path = Path.Combine(CurrentDir, CurrentFile);

        GameManager.DataManager.SaveProjectAs(path);
    }
}
