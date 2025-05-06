using UnityEngine;

public class eduWallCollider : MonoBehaviour
{
    enum WallType {Left, Right, Bottom, Top}

    [SerializeField] WallType wallType = WallType.Top;
    [SerializeField] bool draw = true;

    void Start()
    {
        
        // Vector3 horizontalLine = new Vector3(transform.position.x-100, transform.position.x+100);
        // Vector3 verticalLine = new Vector3(transform.position.y-100, transform.position.y+100);

        
    }

    bool isVertical()
    {
        return wallType == WallType.Right || wallType == WallType.Left;
    }

    bool isHorizontal()
    {
        return wallType == WallType.Top || wallType == WallType.Bottom;
    }

    void Draw()
    {
        if(isVertical())
        {
            Vector3 verticalStartPosition = new Vector3(transform.position.x, transform.position.y-100);
            Vector3 verticalEndPosition = new Vector3(transform.position.x, transform.position.y+100);
            Debug.DrawLine(verticalStartPosition, verticalEndPosition, Color.red);
            return;
        } 

        if(isHorizontal())
        {
            Vector3 horizontalStartPosition = new Vector3(transform.position.x-100, transform.position.y);
            Vector3 horizontalEndPosition = new Vector3(transform.position.x+100, transform.position.y);
            Debug.DrawLine(horizontalStartPosition, horizontalEndPosition, Color.red);
            return;
        }
    }

    void FixedUpdate()
    {
        if(draw) Draw();
    }
}
