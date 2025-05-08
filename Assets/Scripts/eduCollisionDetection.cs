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

public class Contact
{
    eduRigidBody body;
    eduRigidBody other;
    Vector3 collisionNormal; // body->other
    float penetration;

    public Contact()
    {

    }

    public Contact(eduRigidBody body, eduRigidBody other, Vector3 collisionNormal, float penetration)
    {
        this.body = body;
        this.other = other;
        this.collisionNormal = collisionNormal;
        this.penetration = penetration;
    }

    public void Solve()
    {
        Assert.IsTrue(circleWall(body, other) || circleCircle(body, other));
        
        Vector2 impulse = ((body.mass * other.mass) / (body.mass + other.mass)) * (1 + body.restitution) * (other.GetVelocity() - body.GetVelocity()) * collisionNormal;

        //Apply Impulses
        body.applyImpulse(impulse, collisionNormal);
        other.applyImpulse(-impulse, collisionNormal);

        //Overlap Correction
        float errorReduction = 0.8f;
        float Pn = errorReduction * (body.mass * other.mass/(body.mass + other.mass)) * penetration;
        CorrectOverlap(body, -Pn);
        CorrectOverlap(other, Pn);
    }

    bool circleWall(eduRigidBody body, eduRigidBody other)
    {
        bool oneCircle = body.GetComponent<eduCircleCollider>() != null || other.GetComponent<eduCircleCollider>() != null;
        bool oneWall = body.GetComponent<eduWallCollider>() != null || other.GetComponent<eduWallCollider>() != null;

        return oneCircle && oneWall;
    }

    bool circleCircle(eduRigidBody body, eduRigidBody other)
    {
        bool bodyIsCircle = body.GetComponent<eduCircleCollider>() != null;
        bool otherIsCircle = other.GetComponent<eduCircleCollider>() != null;

        return bodyIsCircle && otherIsCircle;
    }

    void CorrectOverlap(eduRigidBody body, float Pn)
    {
        body.transform.position += (Pn/body.mass) * collisionNormal;
    }
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
        bool collision = false;
        // bool horizontalCollision = circle.transform.position.x - wall.transform.position.x <= circle.radius;
        // bool verticalCollision = circle.transform.position.y - wall.transform.position.y <= circle.radius;

        if(wall.isHorizontal()) 
            collision = Math.Abs(circle.transform.position.y - wall.transform.position.y) <= circle.radius;

        if(wall.isVertical())
            collision = Math.Abs(circle.transform.position.x - wall.transform.position.x) <= circle.radius;

        return collision;
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

            if(!MovingApart(circleBody, wallBody)) continue;

            Vector2 distanceVector = wall.transform.position - circleBody.transform.position;
            Vector2 collisionNormal = distanceVector.normalized;
            float penetration = circle.radius + wall.radius - distanceVector.magnitude;

            Contact contact = new Contact(circleBody, wallBody, collisionNormal, penetration);
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

            if(!MovingApart(circleBody, otherBody)) continue;

            Vector2 distanceVector = other.transform.position - circleBody.transform.position;
            Vector2 collisionNormal = distanceVector.normalized;
            float penetration = circle.radius + other.radius - distanceVector.magnitude;

            Contact contact = new Contact(circleBody, otherBody, collisionNormal, penetration);
            contact.Solve();
        }
    }

    bool MovingApart(eduRigidBody rigidBody, eduRigidBody other)
    {
        Vector2 relativeMovement = other.GetVelocity() - rigidBody.GetVelocity();
        Vector2 distanceVector = rigidBody.transform.position - other.transform.position;
        float distance = Math.Abs(distanceVector.magnitude);

        return relativeMovement.magnitude * distance > 0;
    }
}
