using System;
using System.Collections.Generic;

[Serializable]
public class PaletteData
{
    public Guid id;
    public List<VoxelColor> palleteColors;
    public List<VoxelPrefab> palletePrefabs;
    public PaletteData()
    {
        id = Guid.NewGuid();
        palleteColors = new List<VoxelColor>();
        palletePrefabs = new List<VoxelPrefab>();
    }
}