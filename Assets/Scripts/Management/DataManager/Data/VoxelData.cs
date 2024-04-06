using System.Collections.Generic;
using System;
using Godot;
using System.Linq;

public class VoxelData
{
    public Guid id;
    public Color color;
    public List<string> minecraftIDlist;
}

[Serializable]
public class VoxelColor : VoxelData, IEquatable<VoxelColor>
{
    public VoxelColor()
    {
        id = Guid.NewGuid();
        minecraftIDlist = new List<string>();
    }

    public bool Equals(VoxelColor other)
    {
        return other is not null &&
               Mathf.IsEqualApprox(other.color.R, color.R) &&
               Mathf.IsEqualApprox(other.color.G, color.G) &&
               Mathf.IsEqualApprox(other.color.B, color.B) &&
               Mathf.IsEqualApprox(other.color.A, color.A) &&
               other.minecraftIDlist.SequenceEqual(minecraftIDlist);
    }
}

[Serializable]
public class VoxelPrefab : VoxelData, IEquatable<VoxelPrefab>
{
    public string prefabName;
    public string unityPrefabGuid;
    public string unityPrefabTransformFileId;
    public string godotSceneID;

    public VoxelPrefab()
    {
        id = Guid.NewGuid();
        minecraftIDlist = new List<string>();
    }

    public bool Equals(VoxelPrefab other)
    {
        return other is not null &&
               Mathf.IsEqualApprox(other.color.R, color.R) &&
               Mathf.IsEqualApprox(other.color.G, color.G) &&
               Mathf.IsEqualApprox(other.color.B, color.B) &&
               Mathf.IsEqualApprox(other.color.A, color.A) &&
               other.minecraftIDlist.SequenceEqual(minecraftIDlist) &&

               other.prefabName == prefabName &&
               other.unityPrefabGuid == unityPrefabGuid &&
               other.unityPrefabTransformFileId == unityPrefabTransformFileId &&
               other.godotSceneID == godotSceneID;
    }
}
