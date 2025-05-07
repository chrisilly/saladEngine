using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface eduCollider
{
    void OnCollide();
}

public class eduCollisionDetection : MonoBehaviour
{
    List<eduCollider> colliders;
    eduWallCollider[] walls;
    eduCircleCollider[] circles;

    void Start()
    {
        colliders = new List<eduCollider>();
        circles = FindObjectsByType<eduCircleCollider>(FindObjectsSortMode.None);
        walls = FindObjectsByType<eduWallCollider>(FindObjectsSortMode.None);

        foreach(eduCircleCollider circle in circles)
            colliders.Add(circle);
        foreach(eduWallCollider wall in walls)
            colliders.Add(wall);

        foreach(eduCollider collider in colliders)
        {
            Debug.Log($"{collider} loaded.");
        }
    }

    void FixedUpdate()
    {
        foreach(eduCircleCollider circle in circles)
            foreach(eduWallCollider wall in walls)
                if(CircleWallCollision(circle, wall))
                {
                    circle.OnCollide();
                    break;
                }
    }

    bool CircleWallCollision(eduCircleCollider circle, eduWallCollider wall)
    {
        bool collision = false;
        // bool horizontalCollision = circle.transform.position.x - wall.transform.position.x <= circle.radius;
        // bool verticalCollision = circle.transform.position.y - wall.transform.position.y <= circle.radius;

        if(wall.isHorizontal()) 
            collision = Math.Abs(circle.transform.position.y - wall.transform.position.y) <= circle.radius;

        if(wall.isVertical())
            collision = Math.Abs(circle.transform.position.x - wall.transform.position.x) <= circle.radius;

        return collision;
    }
    
    bool CircleCircleCollision(eduCircleCollider circle, eduCircleCollider other)
    {


        return false;
    }
}
