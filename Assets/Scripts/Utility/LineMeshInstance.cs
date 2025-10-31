using Godot;
using System.Collections.Generic;

public partial class LineMeshInstance : MeshInstance3D
{
    private ImmediateMesh immediateMesh;
    private StandardMaterial3D material;

    /// <summary>
    /// Draws a polyline connecting the given points (line strip).
    /// Overwrites previous surface.
    /// </summary>
    public void DrawLineStrip(List<Vector3> points, Color color)
    {
        SetupIfNeeded();

        material.AlbedoColor = color;

        immediateMesh.ClearSurfaces();
        if (points == null || points.Count < 2) { return; }

        // Begin a line-strip primitive (connect each vertex to the next)
        immediateMesh.SurfaceBegin(Mesh.PrimitiveType.LineStrip);
        immediateMesh.SurfaceSetColor(color);

        foreach (Vector3 point in points)
        {
            immediateMesh.SurfaceAddVertex(point);
        }

        immediateMesh.SurfaceEnd();
    }

    /// <summary>
    /// Draws disconnected line segments: every pair of points is a segment.
    /// </summary>
    public void DrawLines(List<(Vector3 pointA, Vector3 pointB)> segments, Color color)
    {
        SetupIfNeeded();

        material.AlbedoColor = color;

        immediateMesh.ClearSurfaces();
        if (segments == null || segments.Count == 0) { return; }

        // PRIMITIVE_LINES: each pair of vertices makes a line
        immediateMesh.SurfaceBegin(Mesh.PrimitiveType.Lines);
        immediateMesh.SurfaceSetColor(color);

        foreach ((Vector3 pointA, Vector3 pointB) in segments)
        {
            immediateMesh.SurfaceAddVertex(pointA);
            immediateMesh.SurfaceAddVertex(pointB);
        }

        immediateMesh.SurfaceEnd();
        
        SetSurfaceOverrideMaterial(0, material);
    }

    private void SetupIfNeeded()
    {
        if (immediateMesh != null) { return; }

        immediateMesh = new();
        Mesh = immediateMesh;

        material = new()
        {
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            DisableReceiveShadows = true,
            ShadowToOpacity = false,
            DisableAmbientLight = true,
            VertexColorUseAsAlbedo = true,
            AlbedoColor = Colors.White
        };

        CastShadow = ShadowCastingSetting.Off;
    }
}