using Godot;

public partial class ShowButton : BaseButton
{
    private Window windowChild;

    public override void _Ready()
    {
        windowChild = this.GetChildByType<Window>();
        Pressed += windowChild.Show;
    }
}

