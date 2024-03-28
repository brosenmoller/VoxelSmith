using fNbt;
using Godot;
using System.Linq;
using System.Collections.Generic;

public class ImportManager : Manager
{
    private void SaveImportSettings(string path)
    {
        EditorData.ImportSettings importSettings = new()
        {
            path = path,
            importType = 0
        };

        if (GameManager.DataManager.EditorData.importPaths.ContainsKey(GameManager.DataManager.ProjectData.id))
        {
            GameManager.DataManager.EditorData.importPaths[GameManager.DataManager.ProjectData.id] = importSettings;
        }
        else
        {
            GameManager.DataManager.EditorData.importPaths.Add(GameManager.DataManager.ProjectData.id, importSettings);
        }

        GameManager.DataManager.SaveEditorData();
    }

    public void ImportMinecraftSchematic(string path, bool saveImportSettings = true)
    {
        MinecraftSchematic schematic = MinecraftSchematic.FromPath(path);

        int airValue = int.MaxValue;
        if (schematic.palette.ContainsValue("minecraft:air"))
        {
            airValue = schematic.palette.FirstOrDefault(x => x.Value == "minecraft:air").Key;
        }

        Dictionary<string, VoxelColor> minecraftIDsToVoxelColor = new();

        for (int i = 0; i < GameManager.DataManager.PaletteData.palleteColors.Count; i++)
        {
            VoxelColor voxelColor = GameManager.DataManager.PaletteData.palleteColors[i];

            for (int j = 0; j < voxelColor.minecraftIDlist.Count; j++)
            {
                string minecraftID = voxelColor.minecraftIDlist[j];

                if (!minecraftIDsToVoxelColor.ContainsKey(voxelColor.minecraftIDlist[j]))
                {
                    minecraftIDsToVoxelColor.Add(voxelColor.minecraftIDlist[j], voxelColor);
                }
            }
        }

        for (int y = 0; y < schematic.height; y++)
        {
            for (int x = 0; x < schematic.height; x++)
            {
                for (int z = 0; z < schematic.height; z++)
                {
                    int index = (y * schematic.length + z) * schematic.width + x;

                    if (index > schematic.blockData.Count) { continue; }

                    int blockValue = schematic.blockData[index];

                    if (blockValue == airValue) { continue; }

                    string minecraftID = schematic.palette[blockValue];

                    if (minecraftIDsToVoxelColor.ContainsKey(minecraftID))
                    {
                        GameManager.DataManager.ProjectData.voxelColors.Add(
                            new Vector3I(x, y, z),
                            minecraftIDsToVoxelColor[minecraftID]
                        );
                    }
                    else
                    {
                        GameManager.DataManager.ProjectData.voxelColors.Add(
                            new Vector3I(x, y, z),
                            GameManager.DataManager.PaletteData.palleteColors.Count > 0 ? 
                                GameManager.DataManager.PaletteData.palleteColors[0] : 
                                new VoxelColor()
                        );
                    }
                }
            }
        }

        if (saveImportSettings)
        {
            SaveImportSettings(path);
        }
    }

    public class MinecraftSchematic
    {
        public short width;
        public short height;
        public short length;
        public List<byte> blockData;
        public Dictionary<int, string> palette;

        public static MinecraftSchematic FromPath(string path)
        {
            NbtFile schamticNBTFile = new();
            schamticNBTFile.LoadFromFile(path);
            NbtCompound compoundTag = schamticNBTFile.RootTag;

            MinecraftSchematic schematic = new()
            {
                width = compoundTag.Get<NbtShort>("Width").ShortValue,
                height = compoundTag.Get<NbtShort>("Height").ShortValue,
                length = compoundTag.Get<NbtShort>("Length").ShortValue,
                palette = new Dictionary<int, string>(compoundTag.Get<NbtInt>("PaletteMax").IntValue)
            };

            byte[] unfilteredBlockData = compoundTag.Get<NbtByteArray>("BlockData").ByteArrayValue;
            schematic.blockData = new List<byte>(schematic.width * schematic.height * schematic.length);

            GD.Print(schematic.blockData.Count);

            for (int i = 0; i < unfilteredBlockData.Length; i++)
            {
                byte blockValue = unfilteredBlockData[i];

                if (blockValue >= 0)
                {
                    schematic.blockData.Add(blockValue);
                }
            }

            foreach (NbtTag tag in compoundTag.Get<NbtCompound>("Palette"))
            {
                string minecraftID = tag.Name;
                if (minecraftID.Contains('['))
                {
                    minecraftID = minecraftID[..minecraftID.IndexOf('[')];
                }

                schematic.palette.Add(tag.IntValue, minecraftID);
            }

            return schematic;
        }
    }
}

