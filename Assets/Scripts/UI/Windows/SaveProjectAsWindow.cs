using Godot;
using System;

public partial class SaveProjectAsWindow : FileDialog
{
    public override void _Ready()
    {
        Confirmed += OnConfirmed;
    }

    private void OnConfirmed()
    {
        
    }
}
