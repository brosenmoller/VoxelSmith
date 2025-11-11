using System.Collections.Generic;
using System;
using Godot;
using Newtonsoft.Json;

public abstract class VoxelData
{
    public Guid id;
    public Color color;
    public List<string> referenceIds;
}

[Serializable, JsonConverter(typeof(VoxelDataConverter))]
public class VoxelColor : VoxelData
{
    public VoxelColor()
    {
        id = Guid.NewGuid();
        referenceIds = [];
    }
}

[Serializable, JsonConverter(typeof(VoxelDataConverter))]
public class VoxelPrefab : VoxelData
{
    public string prefabName;
    public string unityPrefabGuid;
    public string unityPrefabTransformFileId;
    public string godotSceneID;

    public VoxelPrefab()
    {
        id = Guid.NewGuid();
        referenceIds = [];
    }
}