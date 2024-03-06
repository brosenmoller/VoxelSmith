using System;
using System.Collections.Generic;
using Godot;

[Serializable]
public class PaletteData
{
    public Guid id;
    public List<PalleteColor> palleteColors;

    public PaletteData()
    {
        id = Guid.NewGuid();
        palleteColors = new List<PalleteColor>();
    }

    [Serializable]
    public class PalleteColor
    {
        public Color Color;
        public string[] minecraftIDlist;
    }

    //[Serializable]
    //public class PalletePrefab
    //{
    //    public 
    //}

}
