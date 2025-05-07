using UnityEngine;

public class eduWallCollider : MonoBehaviour, eduCollider
{
    enum WallType {Left, Right, Bottom, Top}

    [SerializeField] WallType wallType = WallType.Top;
    [SerializeField] bool draw = true;
    [SerializeField] float length = 50.0f;
    [SerializeField] Color color = Color.red;

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

    void Draw()
    {
        if(isVertical())
        {
            Vector3 verticalStartPosition = new Vector3(transform.position.x, transform.position.y-length/2);
            Vector3 verticalEndPosition = new Vector3(transform.position.x, transform.position.y+length/2);
            Debug.DrawLine(verticalStartPosition, verticalEndPosition, color);
            return;
        } 

        if(isHorizontal())
        {
            Vector3 horizontalStartPosition = new Vector3(transform.position.x-length/2, transform.position.y);
            Vector3 horizontalEndPosition = new Vector3(transform.position.x+length/2, transform.position.y);
            Debug.DrawLine(horizontalStartPosition, horizontalEndPosition, color);
            return;
        }
    }

    void FixedUpdate()
    {
        if(draw) Draw();
    }

    public void OnCollide()
    {
        // We don't actually want the wall to respond in any way to collision... unless?
    }
}
