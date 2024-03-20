using Godot;

public partial class FPSCounter : RichTextLabel
{
    public override void _Process(double delta)
    {
        Text = "FPS: " + Engine.GetFramesPerSecond();
    }
}
