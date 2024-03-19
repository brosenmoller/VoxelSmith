using Godot;
using System.Collections.Generic;

public partial class PaletteUI : BoxContainer
{
    [Export] private PackedScene colorSwatch;
    [Export] private HFlowContainer paletteColorsContainer;
    [Export] private HFlowContainer palettePrefabsContainer;

    public void Update()
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

            StyleBoxFlat normalStyleBox = new() {
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

            button.Pressed += () =>
            {
                GameManager.DataManager.ProjectData.selectedPaletteIndex = 0;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchIndex = index;
            };

            paletteColorsContainer.AddChild(button);
        }
    }
}
