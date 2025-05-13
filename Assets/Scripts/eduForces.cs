using UnityEngine;
using System;
using NUnit.Framework.Constraints;

public class eduForces : MonoBehaviour
{
    eduRigidBody[] rigidBodies;

    public float gravity = -9.81f; 
    public bool applyGravity = true;
    /// <summary> Defaults to down; (0, -1) </summary>
    [SerializeField] Vector2 gravityDirection = Vector2.down;
    
    public float linearDragCoefficient = 0f;
    public bool applyLinearDrag = false; 
    
    public float torque = 0f; 
    public bool applyTorque = false; 
    
    public float angularDragCoefficient = 0f;
    public bool applyAngularDrag = false; 

    //public float windMagnitude = 0f;
    //public Vector2 windDirection = Vector2.zero;
    //public float airDensity;
    //public bool applyWind = false;
    //MOVED TO WINDMAP

    public eduWindMap windMap; //Reference to the wind map script

    public Vector2 buoyancyForce = new Vector2(0,0);
    public float fluidDensity = 0f;
    public float waterLevel = 0f; //How high the water is
    public bool applyBuoyancy = false;
    public float submergedVolume = 0f;

    int applied;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBodies = FindObjectsByType<eduRigidBody>(FindObjectsSortMode.None); //FindObjectsByType<eduRigidBody>(FindObjectsSortMode.None);
        windMap = GetComponentInParent<eduWindMap>();
    }

    void FixedUpdate()
    {
        foreach (eduRigidBody rigidBody in rigidBodies)
        {
            float gravityMagnitude = Math.Abs(gravity * rigidBody.mass * (applied = applyGravity ? 1:0));
            Vector2 gravityForce = gravityDirection * gravityMagnitude;

            float Torques = torque * (applied = applyTorque ? 1 : 0);

            rigidBody.applyForce(gravityForce); // Apply gravity to each rigid body, activate and deactivate with a bool. Alternatively use Convert.ToInt32(applyGravity)
            rigidBody.applyTorque(Torques);     // Apply torque to each rigid body
            rigidBody.applyForce(BuoyancyForce(rigidBody)); //Apply buoyancy to each rigid body
            rigidBody.applyForce(windMap.FindWindForce(rigidBody));
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(new Vector3(-100, waterLevel, 0), new Vector3(100, waterLevel, 0));
        //Draw lines for forces and trajectories
        //Change execution order settings so this runs before eduRigidBody.Update()
    }

    public Vector2 BuoyancyForce(eduRigidBody rigidBody)
    {
        ///<summary> The following section is the math needed for buoyancy. Most likely incomplete and non-functioning. Just so you know.</summary>
        float buoyancyMagnitude = fluidDensity * Mathf.Abs(gravity);
        //density * gravity;
        rigidBody.FindSubmergedHeight(waterLevel);
        //Figure out the submerged height of the object 
        //Since we're trying to find the submerged volume of the object, we clamp between 0 (none of the object is submerged) and the diameter (the whole object is submerged)

        //Find the submerged volume (i.e. area of the segment under water)
        rigidBody.findSegmentArea();

        buoyancyForce.y = buoyancyMagnitude * rigidBody.submergedVolume * (applied = applyBuoyancy ? 1 : 0);
        return buoyancyForce;
    }

    //public Vector2 FindWindForce(eduRigidBody rigidBody)
    //{
    //    Vector2 normalizedWindDirection = windDirection.normalized;

    //    float projectedArea = rigidBody.FindProjectedArea(normalizedWindDirection);

    //    float windForceMagnitude = 0.5f * airDensity * Mathf.Pow(windMagnitude, 2) * projectedArea;

    //    Vector2 windForce = normalizedWindDirection * windForceMagnitude * (applied = applyWind ? 1:0);

    //    Debug.Log($"Wind force: {windForce} (Projected area: {projectedArea})");

    //    return windForce;
    //}
}
