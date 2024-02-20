using Godot;

public static class NodeExtensions
{
    //acts like Unity's GetComponent<T> / GetComponentInChildren<T>
    public static T GetChildByType<T>(this Node node, bool recursive = true) where T : Node
    {
        int childCount = node.GetChildCount();

        for (int i = 0; i < childCount; i++)
        {
            Node child = node.GetChild(i);
            if (child is T childT)
            {
                return childT;
            }

            if (recursive && child.GetChildCount() > 0)
            {
                T recursiveResult = child.GetChildByType<T>(true);
                if (recursiveResult != null)
                    return recursiveResult;
            }
        }

        return null;
    }

    //acts like Unity's GetComponentInParent<T>
    public static T GetParentByType<T>(this Node node) where T : Node
    {
        Node parent = node.GetParent();
        if (parent != null)
        {
            if (parent is T parentT)
            {
                return parentT;
            }
            else
            {
                return parent.GetParentByType<T>();
            }
        }

        return null;
    }
}