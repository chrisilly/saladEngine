using UnityEngine;

public class eduWindMap : MonoBehaviour
{
    public float noiseScale = 0.1f; 
    public float windMagnitude = 1f;
    public Vector2 windDirection = Vector2.zero;
    public float airDensity;
    public bool applyWind = false;
    private float offsetX;
    private float offsetY;

    void Start()
    {
        offsetX = Random.Range(0f, 100f);
        offsetY = Random.Range(0f, 100f);
    }

    void Update()
    {
        offsetX += Time.deltaTime * 0.1f;
        offsetY += Time.deltaTime * 0.2f;
    }

    public Vector2 FindWindForce(eduRigidBody rigidBody)
    {
        if(!applyWind) return Vector2.zero;
        float noiseX = Mathf.PerlinNoise(rigidBody.transform.position.x * noiseScale + offsetX, rigidBody.transform.position.y * noiseScale + offsetY);
        float noiseY = Mathf.PerlinNoise(rigidBody.transform.position.x * noiseScale + offsetX + 100f, rigidBody.transform.position.y * noiseScale + offsetY + 100f);

        windDirection = new Vector2(
            noiseX * 2 - 1,  
            noiseY * 2 - 1
        ).normalized;

        Vector2 normalizedWindDirection = windDirection.normalized;
        float projectedArea = rigidBody.FindProjectedArea(normalizedWindDirection);
        float windForceMagnitude = 0.5f * airDensity * Mathf.Pow(windMagnitude, 2) * projectedArea;
        Vector2 windForce = normalizedWindDirection * windForceMagnitude * (applyWind ? 1 : 0);

        Debug.Log($"WindForce: {windForce}");

        return windForce;
    }

}
