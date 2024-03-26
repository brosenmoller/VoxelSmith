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

        for (int i = 0; i < GameManager.DataManager.PaletteData.palleteColors.Count; i++)
        {
            int index = i;
            VoxelData voxeldata = GameManager.DataManager.PaletteData.palleteColors[index];
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
                BgColor = voxeldata.color / 2,
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

            if (GameManager.DataManager.ProjectData.selectedPaletteIndex == (int)ProjectDataPalleteIndex.COLORS &&
                GameManager.DataManager.ProjectData.selectedPaletteSwatchIndex == index)
            {
                button.ButtonPressed = true;
            }

            button.Pressed += () =>
            {
                GameManager.DataManager.ProjectData.selectedPaletteIndex = (int)ProjectDataPalleteIndex.COLORS;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchIndex = index;
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

        for (int i = 0; i < GameManager.DataManager.PaletteData.palletePrefabs.Count; i++)
        {
            int index = i;
            VoxelPrefab voxeldata = GameManager.DataManager.PaletteData.palletePrefabs[index];
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
                BgColor = voxeldata.color / 2,
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

            if (GameManager.DataManager.ProjectData.selectedPaletteIndex == (int)ProjectDataPalleteIndex.PREFABS && 
                GameManager.DataManager.ProjectData.selectedPaletteSwatchIndex == index)
            {
                button.ButtonPressed = true;
            }

            button.Pressed += () =>
            {
                GameManager.DataManager.ProjectData.selectedPaletteIndex = (int)ProjectDataPalleteIndex.PREFABS;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchIndex = index;
            };

            palettePrefabsContainer.AddChild(button);
        }
    }
}
