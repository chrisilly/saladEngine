using System;
using UnityEngine;

public enum WallPosition { Top, Bottom, Left, Right }

public class eduDumbWallCollider : MonoBehaviour
{
    bool debug = true;


    [SerializeField] Color color = Color.black;
    [SerializeField] WallPosition wallPosition = WallPosition.Bottom;
    float length = 100;

    public void FixedUpdate()
    {
        // Debug.Log($"{this} is {wallType}");
    }

    public Vector2 Normal()
    {
        switch (wallPosition)
        {
            case WallPosition.Top:
                return Vector2.down;
            case WallPosition.Bottom:
                return Vector2.up;
            case WallPosition.Left:
                return Vector2.right;
            case WallPosition.Right:
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }

    public bool Vertical()
    { 
        return wallPosition == WallPosition.Left || wallPosition == WallPosition.Right; 
    }

    public bool Horizontal()
    {
        return wallPosition == WallPosition.Top || wallPosition == WallPosition.Bottom;
    }

    /// <returns>A unit vector pointing in the direction of the wall</returns>
    public Vector2 UnitVector()
    { return new Vector2(Normal().y, -Normal().x); }

    public void OnDrawGizmos()
    {
        Vector2 wallStart = (Vector2)transform.position + UnitVector() * -length;
        Vector2 wallEnd = (Vector2)transform.position + UnitVector() * length;

        Gizmos.color = color;
        Gizmos.DrawLine(wallStart, wallEnd);

        if(debug) 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + UnitVector());
        }
    }
}
