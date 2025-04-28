using UnityEngine;

public class eduRigidBody : MonoBehaviour 
{
    // Position // Fetch position and rotation from Transform
    // Rotation // Fetch position and rotation from Transform
    Vector2 velocity;
    Vector2 angularVelocity;

    [SerializeField] public float mass = 1f;
    float momentOfInertia;

    // force // Accumulator for applied forces
    // torque // Accumulator for applied torques

    public Vector2 Forces; // Accumulator for applied forces
    public float Torques; // Accumulator for applied torques

    [SerializeField] float coefficientOfRestitution = 1f;
    [SerializeField] int skipFrames = 0; // is this really supposed to be in eduRigidBody?

    void FixedUpdate()
    {

    }

    void applyForce(Vector2 force)
    {

    }

    void applyTorque(float torque)
    {

    }

    void applyImpulse(Vector2 impulse)
    {

    }
}
