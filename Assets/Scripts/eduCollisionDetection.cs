using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using Unity.Mathematics;

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
        UpdateCircleWallCollisions();

        UpdateCircleCollisions();
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

    void UpdateCircleWallCollisions()
    {
        foreach(eduCircleCollider circle in circles)
            foreach(eduWallCollider wall in walls)
                if(CircleWallCollision(circle, wall))
                {
                    circle.OnCollide();
                    break;
                }
    }
    
    bool CircleCollision(eduCircleCollider circle, eduCircleCollider other)
    {
        Assert.AreNotEqual(circle, other); // if we pass the same circle colliders in both parameters, we have failed...

        Vector2 distanceVector = circle.transform.position - other.transform.position;
        float distance = Math.Abs(distanceVector.magnitude);
        // float distance = Vector2.Distance(circle.transform.position, other.transform.position);

        bool collision = distance <= circle.radius + other.radius;

        return collision;
    }

    void UpdateCircleCollisions()
    {
        foreach(eduCircleCollider circle in circles)
            foreach(eduCircleCollider other in circles)
                {
                    if(circle == other) continue; // no such thing as collision with oneself

                    if(CircleCollision(circle, other))
                    {
                        circle.OnCollide();
                        other.OnCollide();
                        break;
                    }
                }
    }
}
