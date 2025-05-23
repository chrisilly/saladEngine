using UnityEngine;
using UnityEngine.Assertions;

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

        // Debug.Log($"Contact registered between {body} and {other}. Solving.");
        Debug.DrawLine(body.transform.position, body.transform.position - collisionNormal, Color.white);
    }

    public void Solve()
    {
        Assert.IsTrue(CircleWall(body, other) || CircleCircle(body, other));

        Vector2 relativeVelocity = other.GetVelocity() - body.GetVelocity();
        float restitution = Mathf.Min(body.restitution, other.restitution);

        // Vector2 impulse = body.mass * other.mass / (body.mass + other.mass) * (1 + restitution) * relativeVelocity * collisionNormal; // it was a dot product sign... not a multiplication sign...
        float impulse = Vector2.Dot(body.mass * other.mass / (body.mass + other.mass) * (1 + restitution) * relativeVelocity, collisionNormal);

        //Apply Impulses
        body.applyImpulse(impulse * collisionNormal);
        other.applyImpulse(-impulse * collisionNormal);

        //Overlap Correction
        float errorReduction = 0.5f;
        float Pn = errorReduction * (body.mass * other.mass/(body.mass + other.mass)) * penetration;
        CorrectOverlap(body, -Pn);
        CorrectOverlap(other, Pn);
    }

    void CorrectOverlap(eduRigidBody body, float Pn)
    {
        // if(body.immovable) return;

        float offset = 0f;
        Vector3 correction = (Pn/body.mass) * collisionNormal + offset * ((Pn/body.mass) * collisionNormal).normalized;

		body.transform.position += correction;
		// Debug.Log($"Collision corrected with normal {collisionNormal}");
    }

    /// <returns>Whether or not the contact in question is between a circle and a wall</returns>
    bool CircleWall(eduRigidBody body, eduRigidBody other)
    {
        bool oneCircle = body.GetComponent<eduCircleCollider>() != null || other.GetComponent<eduCircleCollider>() != null;
        bool oneWall = body.GetComponent<eduWallCollider>() != null || other.GetComponent<eduWallCollider>() != null;

        return oneCircle && oneWall;
    }

    /// <returns>Whether or not the contact in question is between two circles</returns>
    bool CircleCircle(eduRigidBody body, eduRigidBody other)
    {
        bool bodyIsCircle = body.GetComponent<eduCircleCollider>() != null;
        bool otherIsCircle = other.GetComponent<eduCircleCollider>() != null;

        return bodyIsCircle && otherIsCircle;
    }
}
