public class ObjFace
{
    public int normalIndex;
    public readonly int[] vertexIndices = new int[6];

    public ObjFace(int normalIndex)
    {
        this.normalIndex = normalIndex;
    }

    public ObjFace(int normalIndex, int[] vertexIndices)
    {
        this.normalIndex = normalIndex;
        this.vertexIndices = vertexIndices;
    }

    public int GetNormal()
    {
        return normalIndex + 1;
    }
}