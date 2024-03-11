using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class ProjectData
{
    public string name;
    public Guid projectID;
    public Guid palleteID;
    public Dictionary<Vector3I, VoxelData> voxels;

    public ProjectData(string name, Guid palleteID)
    {
        this.name = name;
        projectID = Guid.NewGuid();
        this.palleteID = palleteID;
        voxels = new Dictionary<Vector3I, VoxelData>();
    }
}

[Serializable]
public class VoxelData
{

}