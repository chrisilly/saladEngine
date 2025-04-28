using UnityEngine;

public class eduRigidBody : MonoBehaviour 
{
    // Position // Fetch position and rotation from Transform
    // Rotation // Fetch position and rotation from Transform
    
    [SerializeField] Vector2 velocity = Vector2.zero;
    [SerializeField] float angularVelocity = 0;
    [SerializeField] float maxAngularVelocity = 0;

    [SerializeField] public float mass = 1f;
    [SerializeField] float momentOfInertia = 0;
    [SerializeField] float maxMomentOfInertia = 0;

    public Vector2 Forces = Vector2.zero; // Accumulator for applied forces
    public float Torques = 0; // Accumulator for applied torques

    [SerializeField] float restitution = 0.8f;
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
