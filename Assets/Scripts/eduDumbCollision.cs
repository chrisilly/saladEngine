using System;
using UnityEngine;
using UnityEngine.Assertions;

public class eduDumbCollision : MonoBehaviour
{
    bool debug = true;
    eduDumbWallCollider[] walls;
    eduCircleCollider[] circles;

    void Start()
    {
        walls = FindObjectsByType<eduDumbWallCollider>(FindObjectsSortMode.None);
        circles = FindObjectsByType<eduCircleCollider>(FindObjectsSortMode.None);
    }

    void FixedUpdate()
    {
        foreach (eduDumbWallCollider wall in walls)
        {
            foreach (eduCircleCollider circle in circles)
            {
                if(!Collision(circle, wall)) continue;
                if(debug) Debug.Log($"{circle} and {wall} collided!");

                eduRigidBody circleBody = circle.GetComponent<eduRigidBody>();

                // if(!MovingApart(circleBody, wall)) continue;
                
                SolveCollision(circleBody, wall);
            }
        }
    }

    bool Collision(eduCircleCollider circle, eduDumbWallCollider wall)
    {
        return Distance(circle, wall) <= circle.radius;
    }

    void SolveCollision(eduRigidBody circle, eduDumbWallCollider wall)
    {
        Vector2 newVelocity = Vector2.zero;

        // if(debug) Debug.Log($"Changing velocity of {circle} to {newVelocity}.");
        if(wall.Horizontal())
        {
            // Debug.Log($"{wall} is Horizontal? {wall.Horizontal()}. Is Vertical? {wall.Vertical()}");
            newVelocity = new Vector2(circle.GetVelocity().x, -circle.GetVelocity().y);  
            if(debug) Debug.Log($"Changing velocity of {circle} to {newVelocity}.");
        }
        if(wall.Vertical())
        {
            newVelocity = new Vector2(-circle.GetVelocity().x, circle.GetVelocity().y);
            if(debug) Debug.Log($"Changing velocity of {circle} to {newVelocity}.");
        }


        circle.SetVelocity(newVelocity);

        Assert.IsTrue(newVelocity != Vector2.zero, "Weird collision happened!");
    }

    float Distance(eduCircleCollider circle, eduDumbWallCollider wall)
    {
        /// <summary> Hypotenuse of triangle formed by wall origin position, circle position, and the circle's position projected onto the wall </summary>
        Vector2 wallOriginToCircle = wall.transform.position - circle.transform.position;
        /// <summary> Angle between the wall and wallOriginToCircle line </summary>
        float angle = (float)Math.Acos(Vector2.Dot(wallOriginToCircle.normalized, wall.UnitVector()));
        if(angle > 180) angle -= 180; // in case wall.UnitVector() was pointing in the "wrong" direction

        float distance = wallOriginToCircle.magnitude * (float)Math.Sin(angle);

        return distance;
    }

    bool MovingApart(eduRigidBody circle, eduDumbWallCollider wall)
    {
        float angle = (float)Math.Acos(Vector2.Dot(wall.UnitVector(), circle.GetVelocity().normalized));

        return angle < 180;
    }
}
