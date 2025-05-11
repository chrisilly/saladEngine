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

    bool CircleWallCollision(eduCircleCollider circle, eduWallCollider wall)
    {
        if(CircleWallDistance(circle, wall) <= circle.radius) return true;

        return false;
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

    void UpdateCollisions()
    {
        foreach(eduCircleCollider circle in circles)
        {
            UpdateCircleCollisions(circle);
            UpdateWallCollisions(circle);
        }
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

            Contact contact = new(circleBody, wallBody, wall.normal, penetration);
            contact.Solve();
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

            Vector2 distanceVector = other.transform.position - circleBody.transform.position;
            Vector2 collisionNormal = distanceVector.normalized;
            float penetration = circle.radius + other.radius - distanceVector.magnitude;

            Contact contact = new(circleBody, otherBody, collisionNormal, penetration);
            contact.Solve();
        }
    }

    bool MovingApart(eduRigidBody body, eduRigidBody other)
    {
        Vector2 relativeVelocity = other.GetVelocity() - body.GetVelocity();
        float velocityAlongNormal = Vector2.Dot(relativeVelocity, GetCollisionNormal(body, other));
        return velocityAlongNormal > 0;

        Vector2 distanceVector = body.transform.position - other.transform.position;
        float distance = Math.Abs(distanceVector.magnitude);

        return relativeVelocity.magnitude * distance > 0;
    }

    float CircleWallDistance(eduCircleCollider circle, eduWallCollider wall)
    {
        // Line defined defined by point and angle formula (https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line#:~:text=the%20opposite%20vertex.-,Line%20defined%20by%20point%20and%20angle,-%5Bedit%5D)
        float distance =    (float)(
                                Math.Cos(wall.angle) * (wall.transform.position.y - circle.transform.position.y)
                                - Math.Sin(wall.angle) * (wall.transform.position.x - circle.transform.position.x));

        return distance;
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
}
