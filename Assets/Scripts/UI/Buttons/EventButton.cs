using Godot;
using System;

public partial class EventButton : Button
{
    public event Action OnLeftClick;
    public event Action OnRightClick;

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && @event.IsPressed())
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                OnLeftClick?.Invoke();
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                OnRightClick?.Invoke();
            }
        }
    }
}

