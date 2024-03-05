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

    public static PaletteData Default()
    {
        return new PaletteData();
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
