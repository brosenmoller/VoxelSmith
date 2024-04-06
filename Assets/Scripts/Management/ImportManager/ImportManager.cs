using fNbt;
using Godot;
using System.Linq;
using System.Collections.Generic;
using System;

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

        GameManager.DataManager.ProjectData.voxelColors.Clear();
        GameManager.DataManager.ProjectData.voxelPrefabs.Clear();

        int airValue = int.MaxValue;
        if (schematic.palette.ContainsValue("minecraft:air"))
        {
            airValue = schematic.palette.FirstOrDefault(x => x.Value == "minecraft:air").Key;
        }

        Dictionary<string, Guid> minecraftIDsToVoxelData = new();

        foreach (var paletteItem in GameManager.DataManager.PaletteData.paletteColors)
        {
            foreach (string minecraftID in paletteItem.Value.minecraftIDlist)
            {
                minecraftIDsToVoxelData.Add(minecraftID, paletteItem.Key);
            }
        }

        foreach (var paletteItem in GameManager.DataManager.PaletteData.palletePrefabs)
        {
            foreach (string minecraftID in paletteItem.Value.minecraftIDlist)
            {
                minecraftIDsToVoxelData.Add(minecraftID, paletteItem.Key);
            }
        }

        for (int y = 0; y < schematic.height; y++)
        {
            for (int x = 0; x < schematic.width; x++)
            {
                for (int z = 0; z < schematic.length; z++)
                {
                    int index = (y * schematic.length + z) * schematic.width + x;

                    if (index >= schematic.blockData.Count) { continue; }

                    int blockValue = schematic.blockData[index];

                    if (blockValue == airValue) { continue; }

                    string minecraftID = schematic.palette[blockValue];

                    Vector3I position = new(x, y, z);

                    if (minecraftIDsToVoxelData.ContainsKey(minecraftID))
                    {
                        if (GameManager.DataManager.PaletteData.paletteColors.ContainsKey(minecraftIDsToVoxelData[minecraftID]))
                        {
                            GameManager.DataManager.ProjectData.voxelColors.Add(
                                position,
                                minecraftIDsToVoxelData[minecraftID]
                            );
                        }
                        else if (GameManager.DataManager.PaletteData.palletePrefabs.ContainsKey(minecraftIDsToVoxelData[minecraftID]))
                        {
                            GameManager.DataManager.ProjectData.voxelPrefabs.Add(
                                position,
                                minecraftIDsToVoxelData[minecraftID]
                            );
                        }
                    }
                    else if (GameManager.DataManager.PaletteData.paletteColors.Count > 0)
                    {
                        GameManager.DataManager.ProjectData.voxelColors.Add(
                            position,
                            GameManager.DataManager.PaletteData.paletteColors.First().Key
                        );
                    }
                }
            }
        }

        GameManager.SurfaceMesh.UpdateMesh();
        GameManager.PrefabMesh.UpdateMesh();

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
        public List<sbyte> blockData;
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
            schematic.blockData = new List<sbyte>(schematic.width * schematic.height * schematic.length);

            for (int i = 0; i < unfilteredBlockData.Length; i++)
            {
                sbyte blockValue = (sbyte)unfilteredBlockData[i];

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

