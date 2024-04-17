using Godot;
using Godot.Collections;
using System.Collections.Generic;

public static class NodeExtensions
{
    // Acts like Unity's GetComponent<T> and GetComponentInChildren<T>
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
                {
                    return recursiveResult;
                }
            }
        }

        return null;
    }
    // Acts like Unity's GetComponents<T> and GetComponentsInChildren<T>
    public static T[] GetAllChildrenByType<T>(this Node node, bool recursive = true) where T : Node
    {
        List<T> values = new();

        int childCount = node.GetChildCount();

        for (int i = 0; i < childCount; i++)
        {
            Node child = node.GetChild(i);
            if (child is T childT)
            {
                values.Add(childT);
            }

            if (recursive && child.GetChildCount() > 0)
            {
                values.AddRange(child.GetAllChildrenByType<T>());
            }
        }

        return values.ToArray();
    }

    // Acts like Unity's GetComponentInParent<T>
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

    // Acts like Unity's FindObjectOfType<T>
    public static T GetNodeByType<T>(this Node node) where T : Node
    {
        Node rootNode = node.GetTree().Root;
        return rootNode.GetChildByType<T>();
    }

    // Acts like Unity's FindObjectsOfType<T>
    public static T[] GetAllNodesByType<T>(this Node node) where T : Node
    {
        Node rootNode = node.GetTree().Root;
        return rootNode.GetAllChildrenByType<T>();
    }

    public static bool RayCast2D(this CanvasItem node, Vector2 startPosition, Vector2 endPosition, out RayCastHitInfo2D hitInfo, uint layermask = 0xffffffff, bool collideWithAreas = true, bool collideWithBodies = true)
    {

        PhysicsRayQueryParameters2D query = new()
        {
            CollideWithAreas = collideWithAreas,
            CollideWithBodies = collideWithBodies,
            HitFromInside = false,
            From = startPosition,
            To = endPosition,
            CollisionMask = layermask,
        };

        return RayCast2D(node, query, out hitInfo);
    }

    public static bool RayCast2D(this CanvasItem node, PhysicsRayQueryParameters2D query, out RayCastHitInfo2D hitInfo)
    {
        PhysicsDirectSpaceState2D spaceState = node.GetWorld2D().DirectSpaceState;
        Dictionary result = spaceState.IntersectRay(query);

        hitInfo = new();

        if (result == null || result.Count <= 0)
        {
            return false;
        }

        foreach (Variant key in result.Keys)
        {
            switch (key.ToString())
            {
                case "position": hitInfo.position = (Vector2)result[key]; break;
                case "normal": hitInfo.normal = (Vector2)result[key]; break;
                case "collider": hitInfo.collider = (GodotObject)result[key]; break;
                case "collider_id": hitInfo.colliderID = (int)result[key]; break;
                case "rid": hitInfo.rid = (Rid)result[key]; break;
                case "shape": hitInfo.shape = (int)result[key]; break;
            }
        }

        return true;
    }

    public struct RayCastHitInfo2D
    {
        public Vector2 position;
        public Vector2 normal;
        public GodotObject collider;
        public int colliderID;
        public Rid rid;
        public int shape;
    }


    public static bool RayCast3D(this Node3D node, Vector3 startPosition, Vector3 endPosition, out RayCastHitInfo3D hitInfo, uint collisionMask = 0xffffffff, bool collideWithAreas = true, bool collideWithBodies = true)
    {
        PhysicsRayQueryParameters3D query = new()
        {
            CollideWithAreas = collideWithAreas,
            CollideWithBodies = collideWithBodies,
            HitFromInside = false,
            From = startPosition,
            To = endPosition,
            CollisionMask = collisionMask,
        };

        return RayCast3D(node, query, out hitInfo);
    }

    public static bool RayCast3D(this Node3D node, PhysicsRayQueryParameters3D query, out RayCastHitInfo3D hitInfo)
    {
        PhysicsDirectSpaceState3D spaceState = node.GetWorld3D().DirectSpaceState;
        Dictionary result = spaceState.IntersectRay(query);

        hitInfo = new();

        if (result == null || result.Count <= 0)
        {
            return false;
        }

        foreach (Variant key in result.Keys)
        {
            switch (key.ToString())
            {
                case "position": hitInfo.position = (Vector3)result[key]; break;
                case "normal": hitInfo.normal = (Vector3)result[key]; break;
                case "collider": hitInfo.collider = (GodotObject)result[key]; break;
                case "collider_id": hitInfo.colliderID = (int)result[key]; break;
                case "rid": hitInfo.rid = (Rid)result[key]; break;
                case "shape": hitInfo.shape = (int)result[key]; break;
            }
        }

        return true;
    }

    public struct RayCastHitInfo3D
    {
        public Vector3 position;
        public Vector3 normal;
        public GodotObject collider;
        public int colliderID;
        public Rid rid;
        public int shape;
    }
}