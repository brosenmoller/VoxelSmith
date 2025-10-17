using Godot;

public partial class CompassController : Control
{
    [Export] private RichTextLabel text;
    [Export] private Camera3D playerCamera;


    public override void _Process(double delta)
    {
        if (playerCamera == null || text == null) { return; }

        Vector3 forward = -playerCamera.GlobalTransform.Basis.Z;
        string direction = GetCompassDirection(forward);

        text.Text = direction;
    }

    private string GetCompassDirection(Vector3 forward)
    {
        if (Mathf.Abs(forward.X) > Mathf.Abs(forward.Z))
        {
            return forward.X > 0 ? "+X" : "-X";
        }
        else
        {
            return forward.Z > 0 ? "+Z" : "-Z";
        }
    }
}
