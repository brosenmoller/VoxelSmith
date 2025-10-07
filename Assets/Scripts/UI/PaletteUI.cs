using Godot;
using System;
using System.Linq;

public partial class PaletteUI : BoxContainer
{
    [Export] private PackedScene colorSwatch;
    [Export] private PackedScene prefabSwatch;

    [Export] private HFlowContainer paletteColorsContainer;
    [Export] private HFlowContainer palettePrefabsContainer;

    [Export] private NewPaletteColorWindow newPaletteColorWindow;
    [Export] private NewPalettePrefabWindow newPalettePrefabWindow;

    [Export] private Button newPaletteColorButton;
    [Export] private Button newPalettePrefabButton;

    private int lastColorSwatchIndex = 0;
    private int lastPrefabSwatchIndex = 0;

    public override void _Ready()
    {
        newPaletteColorButton.Pressed += newPaletteColorWindow.ShowCreateWindow;
        newPalettePrefabButton.Pressed += newPalettePrefabWindow.ShowCreateWindow;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("cycle_swatch_forward") && !Input.IsKeyPressed(Key.Ctrl))
        {
            CycleSwatch(true);
        }

        if (Input.IsActionJustPressed("cycle_swatch_back") && !Input.IsKeyPressed(Key.Ctrl))
        {
            CycleSwatch(false);
        }

        if (Input.IsActionJustPressed("swap_palette_type"))
        {
            SwapPaletteType();
        }
    }

    private void SwapPaletteType()
    {
        if (GameManager.DataManager.ProjectData.selectedPaletteType == PaletteType.Color)
        {
            if (GameManager.DataManager.PaletteData.palletePrefabs.Count > 0)
            {
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId = GameManager.DataManager.PaletteData.palletePrefabs.Keys.ElementAt(lastPrefabSwatchIndex);
                GameManager.DataManager.ProjectData.selectedPaletteType = PaletteType.Prefab;
                Update();
            }
        }
        else
        {
            if (GameManager.DataManager.PaletteData.paletteColors.Count > 0)
            {
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId = GameManager.DataManager.PaletteData.paletteColors.Keys.ElementAt(lastColorSwatchIndex);
                GameManager.DataManager.ProjectData.selectedPaletteType = PaletteType.Color;
                Update();
            }
        }
    }

    private void CycleSwatch(bool forward)
    {
        Guid[] guidArray;

        if (GameManager.DataManager.ProjectData.selectedPaletteType == PaletteType.Color)
        {
            if (GameManager.DataManager.PaletteData.paletteColors.Count <= 1) { return; }

            guidArray = GameManager.DataManager.PaletteData.paletteColors.Keys.ToArray();
        }
        else
        {
            if (GameManager.DataManager.PaletteData.palletePrefabs.Count <= 1) { return; }
            
            guidArray = GameManager.DataManager.PaletteData.palletePrefabs.Keys.ToArray();
        }

        int index = Array.IndexOf(guidArray, GameManager.DataManager.ProjectData.selectedPaletteSwatchId);

        if (forward)
        {
            if (index == guidArray.Length - 1) { index = 0; }
            else { index++; }
        }
        else
        {
            if (index == 0) { index = guidArray.Length - 1; }
            else { index--; }
        }

        GameManager.DataManager.ProjectData.selectedPaletteSwatchId = guidArray[index];
        Update();
    }

    public void Update()
    {
        UpdateVoxelColorPalette();
        UpdateVoxelPrefabPalette();

        if (lastColorSwatchIndex < 0 || lastColorSwatchIndex >= GameManager.DataManager.PaletteData.paletteColors.Count)
        {
            lastColorSwatchIndex = 0;
        }

        if (lastPrefabSwatchIndex < 0 || lastPrefabSwatchIndex >= GameManager.DataManager.PaletteData.palletePrefabs.Count)
        {
            lastPrefabSwatchIndex = 0;
        }
    }

    private void UpdateVoxelColorPalette()
    {
        for (int i = 0; i < paletteColorsContainer.GetChildren().Count; i++)
        {
            paletteColorsContainer.GetChildren()[i].QueueFree();
        }

        foreach (var paletteColor in GameManager.DataManager.PaletteData.paletteColors)
        {
            VoxelData voxeldata = paletteColor.Value;
            EventButton button = colorSwatch.Instantiate<EventButton>();

            StyleBoxFlat normalStyleBox = new()
            {
                BgColor = voxeldata.color,
                DrawCenter = true,
                CornerRadiusBottomLeft = 5,
                CornerRadiusBottomRight = 5,
                CornerRadiusTopLeft = 5,
                CornerRadiusTopRight = 5
            };

            StyleBoxFlat hoverStyleBox = new()
            {
                BgColor = voxeldata.color / 2,
                DrawCenter = true,
                CornerRadiusBottomLeft = 5,
                CornerRadiusBottomRight = 5,
                CornerRadiusTopLeft = 5,
                CornerRadiusTopRight = 5
            };

            StyleBoxFlat pressedStyleBox = new()
            {
                BgColor = voxeldata.color,
                BorderColor = Color.Color8(255, 255, 255),
                BorderWidthBottom = 2,
                BorderWidthRight = 2,
                BorderWidthLeft = 2,
                BorderWidthTop = 2,
                DrawCenter = true,
                CornerRadiusBottomLeft = 5,
                CornerRadiusBottomRight = 5,
                CornerRadiusTopLeft = 5,
                CornerRadiusTopRight = 5
            };

            button.AddThemeStyleboxOverride("normal", normalStyleBox);
            button.AddThemeStyleboxOverride("pressed", pressedStyleBox);
            button.AddThemeStyleboxOverride("hover", hoverStyleBox);
            button.AddThemeStyleboxOverride("hover pressed", hoverStyleBox);
            button.AddThemeStyleboxOverride("focus", hoverStyleBox);

            if (GameManager.DataManager.ProjectData.selectedPaletteType == PaletteType.Color &&
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId == paletteColor.Key)
            {
                lastColorSwatchIndex = Array.IndexOf(GameManager.DataManager.PaletteData.paletteColors.Keys.ToArray(), paletteColor.Key);
                button.ButtonPressed = true;
            }

            button.OnLeftClick += () =>
            {
                GameManager.DataManager.ProjectData.selectedPaletteType = PaletteType.Color;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId = paletteColor.Key;
            };

            button.OnRightClick += () =>
            {
                newPaletteColorWindow.ShowEditWindow(paletteColor.Key);
            };

            paletteColorsContainer.AddChild(button);
        }
    }

    private void UpdateVoxelPrefabPalette()
    {
        for (int i = 0; i < palettePrefabsContainer.GetChildren().Count; i++)
        {
            palettePrefabsContainer.GetChildren()[i].QueueFree();
        }

        foreach (var palettePrefab in GameManager.DataManager.PaletteData.palletePrefabs)
        {
            VoxelPrefab voxeldata = palettePrefab.Value;
            EventButton button = prefabSwatch.Instantiate<EventButton>();
            RichTextLabel text = button.GetChildByType<RichTextLabel>();

            text.Text = voxeldata.prefabName;

            StyleBoxFlat normalStyleBox = new()
            {
                BgColor = voxeldata.color,
                DrawCenter = true,
                CornerRadiusBottomLeft = 2,
                CornerRadiusBottomRight = 2,
                CornerRadiusTopLeft = 2,
                CornerRadiusTopRight = 2
            };

            StyleBoxFlat hoverStyleBox = new()
            {
                BgColor = voxeldata.color / 2,
                DrawCenter = true,
                CornerRadiusBottomLeft = 2,
                CornerRadiusBottomRight = 2,
                CornerRadiusTopLeft = 2,
                CornerRadiusTopRight = 2
            };

            StyleBoxFlat pressedStyleBox = new()
            {
                BgColor = voxeldata.color,
                BorderColor = Color.Color8(255, 255, 255),
                BorderWidthBottom = 2,
                BorderWidthRight = 2,
                BorderWidthLeft = 2,
                BorderWidthTop = 2,
                DrawCenter = true,
                CornerRadiusBottomLeft = 2,
                CornerRadiusBottomRight = 2,
                CornerRadiusTopLeft = 2,
                CornerRadiusTopRight = 2
            };

            button.AddThemeStyleboxOverride("normal", normalStyleBox);
            button.AddThemeStyleboxOverride("pressed", pressedStyleBox);
            button.AddThemeStyleboxOverride("hover", hoverStyleBox);
            button.AddThemeStyleboxOverride("hover pressed", hoverStyleBox);
            button.AddThemeStyleboxOverride("focus", hoverStyleBox);

            if (GameManager.DataManager.ProjectData.selectedPaletteType == PaletteType.Prefab &&
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId == palettePrefab.Key)
            {
                lastPrefabSwatchIndex = Array.IndexOf(GameManager.DataManager.PaletteData.palletePrefabs.Keys.ToArray(), palettePrefab.Key);
                button.ButtonPressed = true;
            }

            button.OnLeftClick += () =>
            {
                GameManager.DataManager.ProjectData.selectedPaletteType = PaletteType.Prefab;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId = palettePrefab.Key;
            };

            button.OnRightClick += () =>
            {
                newPalettePrefabWindow.ShowEditWindow(palettePrefab.Key);
            };

            palettePrefabsContainer.AddChild(button);
        }
    }
}
