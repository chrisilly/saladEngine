using UnityEngine;

public class eduForces : MonoBehaviour
{
    List<eduRigidBody> rigidBodies = new List<eduRigidBody>();

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBodies = new List<eduRigidBody>(FindObjectsOfType<eduRigidBody>());
    }

    void FixedUpdate()
    {
        foreach (eduRigidBody rb in rigidBodies)
        {
            rb.Forces.Y += gravity * rb.Mass * (int applied = applyGravity ? 1:0);   // Apply gravity to each rigid body, activate and deactivate with a bool. Alternatively use Convert.ToInt32(applyGravity)
            rb.Torques += torque * (applied = applyTorque ? 1:0);               // Apply torque to each rigid body
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Draw lines for forces and trajectories
        //Change execution order settings so this runs before eduRigidBody.Update()
    }
}
