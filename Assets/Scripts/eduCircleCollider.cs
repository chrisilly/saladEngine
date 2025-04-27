using UnityEngine;

public class eduCircleCollider : MonoBehaviour
{
    public float radius = 1.0f;
    public float density = 1.0f;

    public float mass;
    public float momentOfInertia;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        radius = GetComponentInParent<transform>.scale;
        mass = density * Mathf.PI * radius^2;
        momentOfInertia = (mass * radius^4) / 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
