using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ProjectData
{
    public string name;
    public Guid id;

    public Vector3 playerPosition;
    public Vector3 cameraRotation;
    public Vector3 cameraPivotRotation;

    public Dictionary<Vector3I, Guid> voxelColors;
    public Dictionary<Vector3I, Guid> voxelPrefabs;

    public PaletteData palette;
    public PaletteType selectedPaletteType;
    public Guid selectedPaletteSwatchId;
    public ExportSettingsData exportSettings;

    public List<Vector3> snappingPoints;

    [JsonIgnore]
    public VoxelData SelectedVoxelData
    {
        get
        {
            if (selectedPaletteType == PaletteType.Prefab && GameManager.DataManager.PaletteData.palletePrefabs.Count > 0)
            {
                if (!GameManager.DataManager.PaletteData.palletePrefabs.ContainsKey(selectedPaletteSwatchId))
                {
                    selectedPaletteSwatchId = GameManager.DataManager.PaletteData.palletePrefabs.First().Key;
                }

                return GameManager.DataManager.PaletteData.palletePrefabs[selectedPaletteSwatchId];
            }
            else if (selectedPaletteType == PaletteType.Color && GameManager.DataManager.PaletteData.paletteColors.Count > 0)
            {
                if (!GameManager.DataManager.PaletteData.paletteColors.ContainsKey(selectedPaletteSwatchId))
                {
                    selectedPaletteSwatchId = GameManager.DataManager.PaletteData.paletteColors.First().Key;
                }

                return GameManager.DataManager.PaletteData.paletteColors[selectedPaletteSwatchId];
            }
            else
            {
                return null;
            }
        }
    }

    public ProjectData() { }

    public ProjectData(string name)
    {
        this.name = name;
        id = Guid.NewGuid();
        voxelColors = [];
        voxelPrefabs = [];
        snappingPoints = [];
    }
}