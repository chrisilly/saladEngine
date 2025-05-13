using System;
using UnityEngine;

public class eduRigidBody : MonoBehaviour 
{
    [SerializeField] public bool immovable = false;

    [SerializeField] Vector2 velocity = Vector2.zero;
    [SerializeField] float angularVelocity = 0;
    [SerializeField] float maxAngularVelocity = 0;

    public float gravityMultiplier = 1.0f;
    /// <summary> Defaults to down; (0, -1) </summary>
    public Vector2 gravityDirection = Vector2.down;

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

    [SerializeField] public float restitution = 0.8f;
    [SerializeField] int skipFrames = 0;
    int frameCounter = 0;
    float timer;

    void FixedUpdate()
    {
        if(immovable) return;

        if(frameCounter < skipFrames)
        {
            ResetKinetics();
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
        ResetKinetics();
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

    public Vector2 GetVelocity() { return velocity; }
    public void SetVelocity(Vector2 value) 
    { 
        Debug.Log($"Velocity changed from {velocity} to {value}");
        velocity = value;
    }

    void ResetKinetics()
    {
        Forces = Vector2.zero;
        Torques = 0;
    }

    public void applyTorque(float torque)
    {
        Torques += torque;
    }

    public void applyImpulse(Vector2 impulse) 
    {
        velocity += (impulse/mass);
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
}
