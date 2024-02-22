using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class State
{
    public Guid id;
    public string name;
    public HashSet<Vector3I> voxels;

    public State(string name)
    {
        id = Guid.NewGuid();
        this.name = name;
        voxels = new HashSet<Vector3I>();
    }
}
