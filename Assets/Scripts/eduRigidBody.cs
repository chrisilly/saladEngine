using System;
using UnityEngine;

public class eduRigidBody : MonoBehaviour 
{
    [SerializeField] Vector2 velocity = Vector2.zero;
    [SerializeField] float angularVelocity = 0;
    [SerializeField] float maxAngularVelocity = 0;

    // Make get and set functions for the public fellas. No need to expose them to the world.
    [SerializeField] public float mass = 1f;
    [SerializeField] public float momentOfInertia = 0;
    [SerializeField] float maxMomentOfInertia = 0;
    [SerializeField] public float area;
    [SerializeField] public float submergedHeight = 0f;
    [SerializeField] public float submergedVolume = 0f;
    bool hitZero;

    public Vector2 Forces = Vector2.zero;
    public float Torques = 0;

    [SerializeField] float restitution = 0.8f;
    [SerializeField] int skipFrames = 0;
    int frameCounter = 0;
    float timer;

    void FixedUpdate()
    {
        if(frameCounter < skipFrames)
        {
            Forces = Vector2.zero;
            Torques = 0;
            frameCounter++;
            return;
        }

        float variableTimeStep = (skipFrames + 1) * Time.fixedDeltaTime;

        // Semi-implicit Euler method, see README.md
        velocity = velocity + variableTimeStep * Forces/mass;
        // We make a Vector3 out of velocity here as a quick fix because transform.position is a Vector3 by default even in a 2D project for whatever reason
        transform.position = transform.position + variableTimeStep * new Vector3(velocity.x, velocity.y, 0);

        //Semi-implicit Euler method, but applied to angular motion.
        angularVelocity = angularVelocity + variableTimeStep * Torques * (1 / momentOfInertia);
        
        float rotation = Mathf.Rad2Deg * angularVelocity * variableTimeStep;
        
        transform.rotation *= Quaternion.Euler(0,0,rotation);

        //Vector3 rotation = new Vector3(0f, 0f, variableTimeStep * (Mathf.Rad2Deg * angularVelocity));
        //NOTE: Works "as intended" in that it adds torques and all that jazz correctly, however it drops straight to -infinity.
        //NOTE: transform.Rotate works based on EULER angles. Angular velocity and such work off of RADIANS. Conversion is necessary.
        Forces = Vector2.zero;
        Torques = 0;
        frameCounter = 0;
    }

    private void Update()
    {
        timer = Time.time;
        if (angularVelocity <= 0 && hitZero == false)
        {
            hitZero = true;
            Debug.Log($"Angular velocity hit 0 at {timer}");
        }
    }

    public void applyForce(Vector2 force)
    {
        Forces += force;
    }

    /// <summary> Use a float if you want to add speed but not change or give a direction </summary>
    public void applyForce(float magnitude)
    {
        Forces += Forces.normalized * magnitude;
    }

    public Vector2 GetVelocity() { return velocity; }

    public void applyTorque(float torque)
    {
        Torques += torque;
    }

    public float findSegmentArea() 
    {
        float radius = GetComponentInParent<eduCircleCollider>().radius;
        float y;

        y = radius - submergedHeight;
        float angle = Mathf.Acos(y / radius);
        float chordLength = 2 * Mathf.Sqrt((radius * radius) - (y * y));
        float sectorArea = area * (2 * angle / (2 * Mathf.PI));
        submergedVolume = sectorArea - (chordLength * y) / 2;
        return submergedVolume;
    }

    public void FindSubmergedHeight(float waterLevel)
    {
        submergedHeight = Math.Clamp(waterLevel - (transform.position.y - GetComponentInParent<eduCircleCollider>().radius), 0.0f, 2 * GetComponentInParent<eduCircleCollider>().radius);
    }

    public float FindProjectedArea(Vector2 windDirection)
    {
        eduCircleCollider ec = GetComponentInParent<eduCircleCollider>();

        float angle = Vector2.SignedAngle(Vector2.right, windDirection.normalized);

        float radius = ec.radius;

        float projectedArea = 2 * radius * MathF.Abs(Vector2.Dot(transform.up, windDirection.normalized));

        return projectedArea;
    }

    /// <summary>
    /// We don't use this because it's listed as an optional method in the assignment doc
    /// </summary>
    void applyImpulse(Vector2 impulse) {}

}
