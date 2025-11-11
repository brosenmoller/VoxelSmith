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

        AddNewVoxelColor(Color.Color8(66, 57, 42), ["cave"]);
        AddNewVoxelColor(Color.Color8(31, 33, 31), ["temple"]);
        AddNewVoxelColor(Color.Color8(120, 120, 120), ["cracked_temple"]);
        AddNewVoxelColor(Color.Color8(129, 42, 37), [BARRIER]);

        return data;
    }
}