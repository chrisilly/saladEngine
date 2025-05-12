using UnityEngine;
using UnityEngine.UIElements;

public class eduCircleCollider : MonoBehaviour, eduCollider
{
    [SerializeField] bool draw = false;

    // Serialize all the public values and make them private. No reason for them to be public.
    public float radius = 1.0f;
    public float density = 1.0f;
    public float area;

    public float mass;
    public float momentOfInertia;

    public eduRigidBody rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponentInParent<eduRigidBody>();
        radius = transform.lossyScale.x/2;
        area = Mathf.PI * radius * radius;
        mass = density * area;
        //momentOfInertia = (Mathf.PI * radius * radius * radius * radius) / 4.0f;
        momentOfInertia = 0.5f * mass * Mathf.Pow(radius, 2);

        rigidBody.mass = mass;
        rigidBody.momentOfInertia = momentOfInertia;
        rigidBody.area = area;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollide()
    {
        Debug.Log($"{name} has collided!");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(draw) Gizmos.DrawSphere(transform.position, radius);
    }
}
