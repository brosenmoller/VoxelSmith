using fNbt;
using System.Collections.Generic;

public class ImportManager : Manager
{
    public override void Setup()
    {
        ImportMinecraftSchematic("C:\\Users\\Ben\\Documents\\VisualStudioProjects\\VoxelSmithProjects\\VoxelSmithRust\\resources\\test_schematics\\small_medieval_house_2.schem");
    }

    public void ImportMinecraftSchematic(string path)
    {
        NbtFile schamticNBTFile = new();
        schamticNBTFile.LoadFromFile(path);
        NbtCompound compoundTag = schamticNBTFile.RootTag;

        MinecraftSchematic schematic = new()
        {
            width = compoundTag.Get<NbtShort>("Width").ShortValue,
            height = compoundTag.Get<NbtShort>("Height").ShortValue,
            length = compoundTag.Get<NbtShort>("Length").ShortValue,
            blockData = compoundTag.Get<NbtByteArray>("BlockData").ByteArrayValue,
            palette = new Dictionary<string, int>(compoundTag.Get<NbtInt>("PaletteMax").IntValue)
        };

        foreach (NbtTag tag in compoundTag.Get<NbtCompound>("Palette"))
        {
            schematic.palette.Add(tag.Name, tag.IntValue);
        }
    }

    public class MinecraftSchematic
    {
        public short width;
        public short height;
        public short length;
        public byte[] blockData;
        public Dictionary<string, int> palette;
    }
}

