using Godot;
using System.Collections.Generic;
using System.Linq;

public class ObjGreedyMesher
{
    private bool CanMerge(ObjFace baseFace, List<ObjFace> faces, int direction, int size, HashSet<ObjFace> processed)
    {
        ObjFace candidate = FindFaceAtOffset(baseFace, faces, size, 0, direction, (direction + 1) % 3);
        return candidate != null && !processed.Contains(candidate);
    }

    private ObjFace FindFaceAtOffset(ObjFace baseFace, List<ObjFace> faces, int uOffset, int vOffset, int u, int v)
    {
        Vector3I offset = new();
        offset[u] = uOffset;
        offset[v] = vOffset;

        return faces.FirstOrDefault(f => f.vertexIndices.SequenceEqual(baseFace.vertexIndices.Select(v => v + offset)));
    }

    private ObjFace CreateMergedFace(ObjFace baseFace, int width, int height, int u, int v)
    {
        // Adjust vertex positions based on width/height expansion
        int[] newVertexIndices = new int[6];
        for (int i = 0; i < 6; i++)
        {
            newVertexIndices[i] = baseFace.vertexIndices[i]; // Modify as needed for expansion
        }

        return new ObjFace(baseFace.normalIndex, newVertexIndices);
    }

    private void GreedyMesh(List<ObjFace> faces)
    {
        HashSet<ObjFace> processed = new();

        for (int i = 0; i < faces.Count; i++)
        {
            if (processed.Contains(faces[i])) { continue; }

            ObjFace startFace = faces[i];
            processed.Add(startFace);

            // Attempt to merge in the U and V directions
            int u = (startFace.normalIndex + 1) % 3; // Get primary merge direction
            int v = (startFace.normalIndex + 2) % 3; // Get secondary merge direction

            int width = 1, height = 1;

            // Expand in U direction
            while (CanMerge(startFace, faces, u, width, processed)) { width++; }

            // Expand in V direction
            while (CanMerge(startFace, faces, v, height, processed)) { height++; }

            // Mark merged faces as processed
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    ObjFace mergedFace = FindFaceAtOffset(startFace, faces, w, h, u, v);
                    if (mergedFace != null)
                    {
                        processed.Add(mergedFace);
                    }
                }
            }

            // Create the new merged face
            ObjFace newFace = CreateMergedFace(startFace, width, height, u, v);
            faces.Add(newFace);
        }

        // Remove old unmerged faces
        faces.RemoveAll(f => processed.Contains(f));
    }
}