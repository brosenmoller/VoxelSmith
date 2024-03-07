using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class ProjectData
{
    public string name;
    public Guid projectID;
    public Guid palleteID;
    public HashSet<Vector3I> voxels;

    public ProjectData(string name, Guid palleteID)
    {
        this.name = name;
        projectID = Guid.NewGuid();
        this.palleteID = palleteID;
        voxels = new HashSet<Vector3I>();
    }
}
