using UnityEngine;
using UnityEngine.UIElements;

public class eduCircleCollider : MonoBehaviour
{
    public float radius = 1.0f;
    public float density = 1.0f;

    public float mass;
    public float momentOfInertia;

    public eduRigidBody rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponentInParent<eduRigidBody>();
        radius = transform.lossyScale.x/2;
        mass = density * Mathf.PI * radius * radius;
        //momentOfInertia = (Mathf.PI * radius * radius * radius * radius) / 4.0f;
        momentOfInertia = 0.5f * mass * Mathf.Pow(radius, 2);

        rigidBody.mass = mass;
        rigidBody.momentOfInertia = momentOfInertia;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
