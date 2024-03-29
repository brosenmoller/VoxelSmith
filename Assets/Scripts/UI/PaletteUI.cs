using Godot;

public partial class PaletteUI : BoxContainer
{
    [Export] private PackedScene colorSwatch;
    [Export] private PackedScene prefabSwatch;

    [Export] private HFlowContainer paletteColorsContainer;
    [Export] private HFlowContainer palettePrefabsContainer;

    public void Update()
    {
        UpdateVoxelColorPalette();
        UpdateVoxelPrefabPalette();
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
            Button button = colorSwatch.Instantiate<Button>();

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
                button.ButtonPressed = true;
            }

            button.Pressed += () =>
            {
                GameManager.DataManager.ProjectData.selectedPaletteType = PaletteType.Color;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId = paletteColor.Key;
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
            Button button = prefabSwatch.Instantiate<Button>();
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
                button.ButtonPressed = true;
            }

            button.Pressed += () =>
            {
                GameManager.DataManager.ProjectData.selectedPaletteType = PaletteType.Prefab;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId = palettePrefab.Key;
            };

            palettePrefabsContainer.AddChild(button);
        }
    }
}
