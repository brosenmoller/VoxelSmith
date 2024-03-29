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

        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() { color = Color.Color8(0, 0, 0) });
        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() { color = Color.Color8(255, 0, 0) });
        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() { color = Color.Color8(0, 255, 0) });
        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() { color = Color.Color8(0, 0, 255) });
        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() { color = Color.Color8(255, 255, 0) });
        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() { color = Color.Color8(255, 0, 255) });
        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() { color = Color.Color8(0, 255, 255) });
        data.paletteColors.Add(Guid.NewGuid(), new VoxelColor() { color = Color.Color8(255, 255, 255) });

        data.palletePrefabs.Add(Guid.NewGuid(), new VoxelPrefab() { 
            color = Color.Color8(100, 200, 0), 
            unityPrefabGuid = "67ce479430c155e4cbcd3bb0ef4f4954", 
            prefabName = "TestSpehere", 
            unityPrefabTransformFileId = "726921523353226827"
        });

        return data;
    }
}