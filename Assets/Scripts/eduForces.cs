using UnityEngine;
using System;

public class eduForces : MonoBehaviour
{
    eduRigidBody[] rigidBodies;

    public float gravity = -9.81f;
    public bool applyGravity = true;
    
    public float linearDragCoefficient = 0f;
    public bool applyLinearDrag = false; 
    
    public float torque = 0f; 
    public bool applyTorque = false; 
    
    public float angularDragCoefficient = 0f;
    public bool applyAngularDrag = false; 

    public float windForce = 0f;
    public bool applyWind = false;
    
    public float fluidDensity = 0f; 
    public float waterLevel = 0f; //How high the water is
    public bool applyBuoyancy = false;

    int applied;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBodies = FindObjectsByType<eduRigidBody>(FindObjectsSortMode.None); //FindObjectsByType<eduRigidBody>(FindObjectsSortMode.None);
    }

    void FixedUpdate()
    {
        foreach (eduRigidBody rigidBody in rigidBodies)
        {
            float gravityMagnitude = Math.Abs(gravity * rigidBody.mass * (applied = applyGravity ? 1:0)) * rigidBody.gravityMultiplier;
            Vector2 gravityForce = rigidBody.gravityDirection * gravityMagnitude;

            float Torques = torque * (applied = applyTorque ? 1 : 0);

            rigidBody.applyForce(gravityForce); // Apply gravity to each rigid body, activate and deactivate with a bool. Alternatively use Convert.ToInt32(applyGravity)
            rigidBody.applyTorque(Torques);     // Apply torque to each rigid body
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Draw lines for forces and trajectories
        foreach (var rigidBody in rigidBodies)
        {
            Debug.DrawLine(rigidBody.transform.position, rigidBody.transform.position + ToVector3(rigidBody.GetVelocity()), Color.green);
        }
        //Change execution order settings so this runs before eduRigidBody.Update()
    }

    static public Vector3 ToVector3(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}
