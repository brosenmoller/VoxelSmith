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
        projectID = Guid.NewGuid();
        this.name = name;
        voxels = new HashSet<Vector3I>();
        this.palleteID = palleteID;
    }
}
