using System.Collections.Generic;
using Godot;

public class PaletteDataFactory
{
    public const string BARRIER = "barrier";

    public static PaletteData GetDefaultPalette()
    {
        PaletteData data = new();

        void AddNewVoxelColor(Color color, List<string> referenceIDs)
        {
            VoxelColor voxelColor = new()
            {
                color = color,
                referenceIds = referenceIDs,
            };
            data.paletteColors.Add(voxelColor.id, voxelColor);
        }

        AddNewVoxelColor(Color.Color8(70, 52, 24), ["cave"]);
        AddNewVoxelColor(Color.Color8(25, 25, 25), ["temple"]);
        AddNewVoxelColor(Color.Color8(95, 95, 95), ["cracked_temple"]);
        AddNewVoxelColor(Color.Color8(133, 40, 35), [BARRIER]);

        return data;
    }
}