using System;
using UnityEngine;

public class eduWallCollider : MonoBehaviour, eduCollider
{
    public enum WallType {Left, Right, Bottom, Top}

    [SerializeField] public WallType wallType = WallType.Top;
    [SerializeField] bool draw = true;
    [SerializeField] float length = 50.0f;
    [SerializeField] Color color = Color.red;
    [SerializeField] Color debug = Color.green;
    public float slope { get { return (float)Math.Tan(angle); } }
    public float offset { get { return slope * transform.position.x - transform.position.y;}}
    public Vector3 startPosition { get { return new Vector3(transform.position.x - (float)Math.Cos(angle)*length, transform.position.y - (float)Math.Sin(angle)*length); } }
    public Vector3 endPosition { get { return new Vector3(transform.position.x - (float)Math.Cos(angle + Math.PI)*length, transform.position.y - (float)Math.Sin(angle + Math.PI)*length); } }
    public Vector3 vector { get { return startPosition-endPosition; } }
    public Vector3 normal { get { return new Vector3(vector.y, -vector.x).normalized; } }
    [Range(0.0f, (float)Math.PI*2.0f)] public float angle = 0.0f;
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

        Gizmos.DrawLine(transform.position, startPosition);
        Gizmos.DrawLine(transform.position, endPosition);

        DrawNormal(vector);
    }

    void DrawNormal(Vector3 line)
    {
        Gizmos.color = debug;

        Vector3 endPosition = transform.position - normal;
        Gizmos.DrawLine(transform.position, endPosition);
        // Gizmos.DrawLine(Vector3.zero, normal);
    }

    public void OnCollide()
    {
        // We don't actually want the wall to respond in any way to collision... unless?
    }
}
