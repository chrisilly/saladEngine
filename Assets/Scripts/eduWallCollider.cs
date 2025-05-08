using System;
using UnityEngine;

public class eduWallCollider : MonoBehaviour, eduCollider
{
    public enum WallType {Left, Right, Bottom, Top}

    [SerializeField] public WallType wallType = WallType.Top;
    [SerializeField] bool draw = true;
    [SerializeField] float length = 50.0f;
    [SerializeField] Color color = Color.red;
    // [SerializeField] float slope = 0.0f;
    [SerializeField] float angle = 0.0f;
    public float radius = 0.0f; // wall thickness

    void Start()
    {
        
    }

    public bool isVertical()
    {
        return wallType == WallType.Right || wallType == WallType.Left;
    }

    public bool isHorizontal()
    {
        return wallType == WallType.Top || wallType == WallType.Bottom;
    }

    void FixedUpdate()
    {
        // if(draw) Draw();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;

        float x = transform.position.x;
        // float y = transform.position.y;

        Vector3 startPosition = new Vector3(x, transform.position.y);
        Vector3 endPosition = new Vector3(transform.position.x - (float)Math.Cos(angle)*length, transform.position.y - (float)Math.Sin(angle)*length);
        Vector3 endPosition2 = new Vector3(transform.position.x - (float)Math.Cos(angle + 3.1415)*length, transform.position.y - (float)Math.Sin(angle + 3.1415)*length);
        // Gizmos.DrawLine(startPosition, endPosition);
        Gizmos.DrawLine(startPosition, endPosition);
        Gizmos.DrawLine(startPosition, endPosition2);
        return;

        if(isVertical() && draw)
        {
            Vector3 verticalStartPosition = new Vector3(transform.position.x, transform.position.y-length/2);
            Vector3 verticalEndPosition = new Vector3(transform.position.x, transform.position.y+length/2);
            Gizmos.DrawLine(verticalStartPosition, verticalEndPosition);
            return;
        } 

        if(isHorizontal() && draw)
        {
            Vector3 horizontalStartPosition = new Vector3(transform.position.x-length/2, transform.position.y);
            Vector3 horizontalEndPosition = new Vector3(transform.position.x+length/2, transform.position.y);
            return;
        }
    }

    public void OnCollide()
    {
        // We don't actually want the wall to respond in any way to collision... unless?
    }
}
