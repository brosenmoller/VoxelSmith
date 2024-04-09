using System.Collections.Generic;
using Godot;

public class PaletteDataFactory
{
    public static PaletteData GetDefaultPalette()
    {
        PaletteData data = new();

        void AddNewVoxelColor(Color color, List<string> minecraftIDlist)
        {
            VoxelColor voxelColor = new()
            {
                color = color,
                minecraftIDlist = minecraftIDlist,
            };
            data.paletteColors.Add(voxelColor.id, voxelColor);
        }

        AddNewVoxelColor(
            Color.Color8(95, 135, 87),
            new List<string>()
            {
                "minecraft:grass_block"
            }
        );

        AddNewVoxelColor(
            Color.Color8(165, 125, 92),
            new List<string>()
            {
                "minecraft:dirt"
            }
        );

        AddNewVoxelColor(
            Color.Color8(120, 120, 120),
            new List<string>()
            {
                "minecraft:stone"
            }
        );

        AddNewVoxelColor(
            Color.Color8(110, 110, 110),
            new List<string>()
            {
                "minecraft:cobblestone"
            }
        );

        AddNewVoxelColor(
            Color.Color8(129, 42, 37),
            new List<string>()
            {
                "minecraft:red_concrete"
            }
        );

        AddNewVoxelColor(
            Color.Color8(43, 45, 137),
            new List<string>()
            {
                "minecraft:blue_concrete"
            }
        );

        AddNewVoxelColor(
            Color.Color8(239, 237, 231),
            new List<string>()
            {
                "minecraft:smooth_quartz"
            }
        );

        return data;
    }
}

