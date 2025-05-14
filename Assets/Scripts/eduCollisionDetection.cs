using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

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
        UpdateCollisions();
    }

    void UpdateCollisions()
    {
        foreach(eduCircleCollider circle in circles)
        {
            UpdateCircleCollisions(circle);
            UpdateWallCollisions(circle);
        }
    }

    void UpdateCircleCollisions(eduCircleCollider circle)
    {
        foreach(eduCircleCollider other in circles)
        {
            if(circle == other) continue; // no such thing as collision with oneself

            if(!CircleCollision(circle, other)) continue;

            eduRigidBody circleBody = circle.GetComponent<eduRigidBody>();
            eduRigidBody otherBody = other.GetComponent<eduRigidBody>();

            if(MovingApart(circleBody, otherBody)) continue;

            float distance = Vector2.Distance(other.transform.position, circle.transform.position);
            float penetration = circle.radius + other.radius - distance;

            Contact contact = new(circleBody, otherBody, GetCollisionNormal(circleBody, otherBody), penetration);
            contact.Solve();
        }
    }

    bool CircleCollision(eduCircleCollider circle, eduCircleCollider other)
    {
        Assert.AreNotEqual(circle, other); // if we pass the same circle colliders in both parameters, we have failed...

        float distance = (circle.transform.position - other.transform.position).magnitude; 

        return distance <= circle.radius + other.radius;
    }

    bool MovingApart(eduRigidBody body, eduRigidBody other)
    {
        float velocityAlongNormal;
        Vector2 relativeVelocity = other.GetVelocity() - body.GetVelocity();
        velocityAlongNormal = Vector2.Dot(relativeVelocity, GetCollisionNormal(body, other));
        // Vector2 relativePosition = other.transform.position - body.transform.position; // Functionally the same thing as GetCollisionNormal()
        // velocityAlongNormal = Vector2.Dot(relativeVelocity, relativePosition);

        return velocityAlongNormal > 0;
    }

    Vector2 GetCollisionNormal(eduRigidBody body, eduRigidBody other)
    {
        Vector2 collisionNormal;
        if(other.GetComponent<eduWallCollider>() != null)
        {
            return other.GetComponent<eduWallCollider>().normal;
        }

        Vector2 distanceVector = other.transform.position - body.transform.position;
        collisionNormal = distanceVector.normalized;

        return collisionNormal;
    }

    void UpdateWallCollisions(eduCircleCollider circle)
    {
        foreach(eduWallCollider wall in walls)
        {
            if(!CircleWallCollision(circle, wall)) continue;

            eduRigidBody circleBody = circle.GetComponent<eduRigidBody>();
            eduRigidBody wallBody = wall.GetComponent<eduRigidBody>();

            if(MovingApart(circleBody, wallBody)) continue;

            float penetration = circle.radius - CircleWallDistance(circle, wall);

            Contact contact = new(circleBody, wallBody, GetCollisionNormal(circleBody, wallBody), penetration);
            contact.Solve();
        }
    }

    bool CircleWallCollision(eduCircleCollider circle, eduWallCollider wall)
    {
        if(CircleWallDistance(circle, wall) <= circle.radius) return true;

        return false;
    }

    float CircleWallDistance(eduCircleCollider circle, eduWallCollider wall)
    {
        // Line defined defined by point and angle formula (https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line#:~:text=the%20opposite%20vertex.-,Line%20defined%20by%20point%20and%20angle,-%5Bedit%5D)
        float distance = (float)(
            Math.Cos(wall.angle) * (wall.transform.position.y - circle.transform.position.y)
            - Math.Sin(wall.angle) * (wall.transform.position.x - circle.transform.position.x));

        return distance;
    }
}
