using Godot;
using System;

public partial class PaletteMenu : PopupMenu
{
    [Export] private Button button;

    public static event Action OnNewPressed;
    public static event Action OnOpenPressed;
    public static event Action OnSavePressed;
    public static event Action OnSaveAsPressed;

    public override void _Ready()
    {
        // Disable Palette Menu for now, for beta
        //button.Pressed += OnClick;


        IdPressed += OnMenuItemSelected;
        SetupMenu();
    }

    private void OnClick()
    {
        Show();
        Position = (Vector2I)button.GetGlobalTransformWithCanvas().Origin;
    }

    private void OnMenuItemSelected(long id)
    {
        switch (id)
        {
            case (long)PalleteOptions.NEW: OnNewPressed?.Invoke(); break;
            case (long)PalleteOptions.OPEN: OnOpenPressed?.Invoke(); break;
            case (long)PalleteOptions.SAVE: OnSavePressed?.Invoke(); break;
            case (long)PalleteOptions.SAVE_AS: OnSaveAsPressed?.Invoke(); break;
        }
    }

    private void SetupMenu()
    {
        // 0
        AddItem("New", (int)PalleteOptions.NEW);

        // 1
        AddItem("Open", (int)PalleteOptions.OPEN);

        // 2
        AddItem("Save", (int)PalleteOptions.SAVE);

        // 3
        AddItem("Save As", (int)PalleteOptions.SAVE_AS);
    }

    private enum PalleteOptions
    {
        NEW,
        OPEN,
        SAVE,
        SAVE_AS,
    }
}
