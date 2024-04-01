using Godot;
using System;
using System.Collections.Generic;

public enum PaletteType
{
    None = 0,
    Color = 1,
    Prefab = 2,
}

[Serializable]
public class PaletteData
{
    public Guid id;
    public Dictionary<Guid, VoxelColor> paletteColors;
    public Dictionary<Guid, VoxelPrefab> palletePrefabs;

    public PaletteData()
    {
        id = Guid.NewGuid();
        paletteColors = new Dictionary<Guid, VoxelColor>();
        palletePrefabs = new Dictionary<Guid, VoxelPrefab>();
    }

    public static PaletteData Default()
    {
        PaletteData data = new();

        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() 
        { 
            color = Color.Color8(95, 135, 87), 
            minecraftIDlist = new List<string>() 
            {
                "minecraft:grass_block"
            },
            id = new Guid()
        });

        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor()
        {
            color = Color.Color8(165, 125, 92),
            minecraftIDlist = new List<string>()
            {
                "minecraft:dirt"
            }
        });

        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor()
        {
            color = Color.Color8(120, 120, 120),
            minecraftIDlist = new List<string>()
            {
                "minecraft:stone"
            }
        });

        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor()
        {
            color = Color.Color8(110, 110, 110),
            minecraftIDlist = new List<string>()
            {
                "minecraft:cobblestone"
            }
        });

        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor()
        {
            color = Color.Color8(129, 42, 37),
            minecraftIDlist = new List<string>()
            {
                "minecraft:red_concrete"
            }
        });

        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor()
        {
            color = Color.Color8(43, 45, 137),
            minecraftIDlist = new List<string>()
            {
                "minecraft:blue_concrete"
            }
        });

        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor()
        {
            color = Color.Color8(239, 237, 231),
            minecraftIDlist = new List<string>()
            {
                "minecraft:smooth_quartz"
            }
        });

        //data.palletePrefabs.Add(Guid.NewGuid(), new VoxelPrefab() { 
        //    color = Color.Color8(100, 200, 0), 
        //    unityPrefabGuid = "67ce479430c155e4cbcd3bb0ef4f4954", 
        //    prefabName = "TestSpehere", 
        //    unityPrefabTransformFileId = "726921523353226827"
        //});

        return data;
    }
}