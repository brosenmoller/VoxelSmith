using Godot;
using System;
using System.Collections.Generic;


[Serializable]
public class ProjectData
{
    public string name;
    public Guid projectID;
    public Guid palleteID;
    public Vector3 playerPosition;
    public Vector3 cameraPivotRotation;
    public Vector3 cameraRotation;
    public Dictionary<Vector3I, VoxelData> voxels;

    public ProjectData() { }

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
    public Color color;

    public VoxelData() 
    {
        color = Color.Color8(255, 0, 0, 255);
    }
}
