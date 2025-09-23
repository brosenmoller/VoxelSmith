using Godot;

public partial class GammaToggle : WorldEnvironment
{
    [Export] private int highGamma;
    [Export] private DirectionalLight3D light;

    private float standardGamma;
    private bool isHighGammaEnabled = false;

    public override void _Ready()
    {
        standardGamma = Environment.BackgroundEnergyMultiplier;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("toggle_gamma"))
        {
            isHighGammaEnabled = !isHighGammaEnabled;
            Environment.BackgroundEnergyMultiplier = isHighGammaEnabled ? highGamma : standardGamma;
            light.ShadowEnabled = !isHighGammaEnabled;
        }
    }
}