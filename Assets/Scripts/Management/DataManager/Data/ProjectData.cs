using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public enum ProjectDataPalleteIndex
{
    COLORS = 0,
    PREFABS = 1,
}


[Serializable]
public class ProjectData
{
    public string name;
    public Guid projectID;
    public Guid palleteID;
    public Vector3 playerPosition;
    public Vector3 cameraRotation;
    public Vector3 cameraPivotRotation;
    public Dictionary<Vector3I, VoxelColor> voxelColors;
    public Dictionary<Vector3I, VoxelPrefab> voxelPrefabs;
    public int selectedPaletteIndex;
    public int selectedPaletteSwatchIndex;

    [JsonIgnore]
    public VoxelData SelectedVoxelData
    {
        get
        {
            if (GameManager.DataManager.PaletteData.palleteColors.Count <= 0 ||
                GameManager.DataManager.PaletteData.palletePrefabs.Count <= 0)
            {
                return null;
            }


            if (selectedPaletteIndex == (int)ProjectDataPalleteIndex.COLORS)
            {
                if (selectedPaletteSwatchIndex >= GameManager.DataManager.PaletteData.palleteColors.Count)
                {
                    selectedPaletteSwatchIndex = GameManager.DataManager.PaletteData.palleteColors.Count - 1;
                }

                return GameManager.DataManager.PaletteData.palleteColors[selectedPaletteSwatchIndex];
            }
            else
            {
                if (selectedPaletteSwatchIndex >= GameManager.DataManager.PaletteData.palletePrefabs.Count)
                {
                    selectedPaletteSwatchIndex = GameManager.DataManager.PaletteData.palletePrefabs.Count - 1;
                }
                return GameManager.DataManager.PaletteData.palletePrefabs[selectedPaletteSwatchIndex];
            }
        }
    }

    public ProjectData() { }

    public ProjectData(string name, Guid palleteID)
    {
        this.name = name;
        projectID = Guid.NewGuid();
        this.palleteID = palleteID;
        voxelColors = new Dictionary<Vector3I, VoxelColor>();
        voxelPrefabs = new Dictionary<Vector3I, VoxelPrefab>();
    }
}