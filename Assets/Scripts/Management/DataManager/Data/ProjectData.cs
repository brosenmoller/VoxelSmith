using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

    //public SaveableProjectData GetSaveData()
    //{
    //    return new SaveableProjectData()
    //    {
    //        name = name,
    //        projectID = projectID,
    //        palleteID = palleteID,
    //        voxelLocations = voxels.Keys.ToList(),
    //        voxelData = voxels.Values.ToList(),
    //    };

    //}

    //[Serializable]
    //public class SaveableProjectData
    //{
    //    public string name;
    //    public Guid projectID;
    //    public Guid palleteID;
    //    public List<Vector3I> voxelLocations;
    //    public List<VoxelData> voxelData;
    //}
}

[Serializable]
public class VoxelData
{

}
