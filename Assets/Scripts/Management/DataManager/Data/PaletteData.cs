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
}