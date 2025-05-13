using System;
using UnityEngine;
using UnityEngine.Assertions;

public class eduDumbCollision : MonoBehaviour
{
    bool debug = false;
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

                if(!MovingApart(circleBody, wall)) continue;
                
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

        if(wall.Horizontal())
        {
            newVelocity = new Vector2(circle.GetVelocity().x, -circle.GetVelocity().y);  
            if(debug) Debug.Log($"Changing velocity of {circle} to {newVelocity}.");
        }
        if(wall.Vertical())
        {
            newVelocity = new Vector2(-circle.GetVelocity().x, circle.GetVelocity().y);
            if(debug) Debug.Log($"Changing velocity of {circle} to {newVelocity}.");
        }

		// Vector2 testVelocity = circle.GetVelocity().magnitude * wall.Normal();
		// Vector2 testVelocity2 = 2 * Vector2.Dot(circle.GetVelocity(), wall.Normal()) * wall.Normal();
		// Vector2 testVelocity3 = circle.GetVelocity() + wall.Normal();
		// Vector2 refectedVelocity = GetNewVelocity(circle, wall);

        circle.SetVelocity(GetNewVelocity(circle, wall));
		// circle.applyImpulse(testVelocity);

		CorrectPosition(circle, wall);

        Assert.IsTrue(newVelocity != Vector2.zero, "Weird collision happened!");
    }

	Vector2 GetNewVelocity(eduRigidBody circle, eduDumbWallCollider wall)
	{
		eduCircleCollider circleCollider = circle.GetComponent<eduCircleCollider>();

		float angle = (float)Math.Acos(Vector2.Dot(-circle.GetVelocity().normalized, wall.UnitVector()));
		if(angle > 180) angle -= 180; // in case wall.UnitVector() was pointing in the "wrong" direction

		float newVelocityComposite = 2 * (float)Math.Sin(angle) * (-circle.GetVelocity()).magnitude;

		Vector2 newVelocity = circle.GetVelocity() + newVelocityComposite * wall.Normal() * circle.restitution; // * vector.magnitude ?

		return newVelocity;
	}

	void CorrectPosition(eduRigidBody circle, eduDumbWallCollider wall)
	{
		eduCircleCollider circleCollider = circle.GetComponent<eduCircleCollider>();

		float errorReduction = 1.0f;
		float penetration = circleCollider.radius - Distance(circleCollider, wall);
		penetration *= Distance(circleCollider, wall) < circleCollider.radius ? 1 : 0;

		circle.transform.position +=  penetration * (Vector3)wall.Normal() * errorReduction;
	}

    float Distance(eduCircleCollider circle, eduDumbWallCollider wall)
    {
        /// <summary> Hypotenuse of triangle formed by wall origin position, circle position, and the circle's position projected onto the wall </summary>
        Vector2 wallOriginToCircle = wall.transform.position - circle.transform.position;
        /// <summary> Angle between the wall and wallOriginToCircle line </summary>
        float angle = (float)Math.Acos(Vector2.Dot(wallOriginToCircle.normalized, wall.UnitVector()));
        if(angle > 180) angle -= 180; // in case wall.UnitVector() was pointing in the "wrong" direction

		// Sin(x) = O / H ==> O = H * Sin(x)
        float distance = wallOriginToCircle.magnitude * (float)Math.Sin(angle);

        return distance;
    }

    bool MovingApart(eduRigidBody circle, eduDumbWallCollider wall)
    {
        float angle = (float)Math.Acos(Vector2.Dot(wall.UnitVector(), circle.GetVelocity().normalized));

        return angle < 180;
    }
}
