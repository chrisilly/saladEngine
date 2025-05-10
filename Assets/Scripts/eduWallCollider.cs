using System;
using UnityEngine;

public class eduWallCollider : MonoBehaviour, eduCollider
{
    public enum WallType {Left, Right, Bottom, Top}

    [SerializeField] public WallType wallType = WallType.Top;
    [SerializeField] bool draw = true;
    [SerializeField] float length = 50.0f;
    [SerializeField] Color color = Color.red;
    public float slope { get { return (float)Math.Tan(angle); } }
    public float offset { get { return slope * transform.position.x - transform.position.y;}}
    [Range((float)-Math.PI/2, (float)Math.PI/2)] public float angle = 0.0f;
    [NonSerialized] public float radius = 0.0f; // wall thickness

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

        DrawNormal(endPosition-endPosition2);
    }

    void DrawNormal(Vector3 line)
    {
        Gizmos.color = Color.grey;

        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y);
        Vector3 endPosition = transform.position + new Vector3(line.y, -line.x);
        Gizmos.DrawLine(startPosition, endPosition);
    }

    public void OnCollide()
    {
        // We don't actually want the wall to respond in any way to collision... unless?
    }
}
