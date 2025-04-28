using UnityEngine;

public class eduRigidBody : MonoBehaviour 
{
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
        float variableTimeStep = (skipFrames + 1) * Time.fixedDeltaTime;

        // Semi-implicit Euler method, see README.md
        velocity = velocity + variableTimeStep * Forces/mass;
        // We make a Vector3 out of velocity here as a quick fix because transform.position is a Vector3 by default even in a 2D project for whatever reason
        transform.position = transform.position + variableTimeStep * new Vector3(velocity.x, velocity.y, 1);

        Forces = Vector2.zero;
        Torques = 0;
    }

    public void applyForce(Vector2 force)
    {
        Forces += force;
    }

    /// <summary> Use a float if you don't want to change or give a Vector2 direction </summary>
    public void applyForce(float magnitude)
    {
        Forces += Forces.normalized * magnitude;
    }

    /// <summary>
    /// All this does is: Torques += torque
    /// </summary>
    /// <param name="torque"></param>
    public void applyTorque(float torque)
    {
        Torques += torque;
    }

    /// <summary>
    /// We don't use this because it's listed as an optional method in the assignment doc
    /// </summary>
    /// <param name="impulse"></param>
    void applyImpulse(Vector2 impulse)
    {

    }
}
