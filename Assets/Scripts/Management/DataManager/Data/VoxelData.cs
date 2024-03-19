using System.Collections.Generic;
using System;
using Godot;

public class VoxelData
{
    public Color color;
}

[Serializable]
public class VoxelColor : VoxelData
{
    public List<string> minecraftIDlist;

    public VoxelColor()
    {
        minecraftIDlist = new List<string>();
    }
}

[Serializable]
public class VoxelPrefab : VoxelData
{
    public string unityPrefabID;
    public string godotSceneID;
}
