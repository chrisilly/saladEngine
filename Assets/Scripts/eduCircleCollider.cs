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
        radius = transform.lossyScale.x;
        mass = density * Mathf.PI * radius * radius;
        momentOfInertia = (mass * radius * radius * radius * radius) / 4.0f;

        rigidBody.mass = mass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
