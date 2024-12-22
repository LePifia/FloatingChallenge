using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    [Tooltip("A stronger fluid force makes the water push back harder against the object.")]
    [SerializeField] float fluidForce = 10f;

    [Tooltip("Drag coefficient to simulate water resistance.")]
    [SerializeField] float dragCoefficient = 0.5f;

    [Tooltip("How frequently the random force factor changes (in frames).")]
    [SerializeField] float frameInterval = 120f;

    float randomWaterForce = 1f;
    float waterLevel;
    Rigidbody rb;
    float immersionDepth;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("[Buoyancy]: No Rigidbody found on the parent object!");
        }
    }

    void FixedUpdate()
    {
        if (WaterLevel.Instance == null) return;

        // Update water level dynamically
        waterLevel = WaterLevel.Instance.currentWaterLevel;

        // Update random force occasionally
        if (Time.frameCount % frameInterval == 0)
        {
            randomWaterForce = Mathf.PerlinNoise(Time.time * 0.1f, 0) + 0.5f; // Smooth variation
        }

        // Calculate immersion depth
        Vector3 position = transform.position;
        if (position.y < waterLevel)
        {
            immersionDepth = waterLevel - position.y;

            // Apply buoyant force proportional to immersion depth
            Vector3 buoyantForce = Vector3.up * immersionDepth * fluidForce * randomWaterForce;
            rb.AddForce(buoyantForce, ForceMode.Force);

            // Apply drag to reduce velocity and simulate water resistance
            ApplyDrag();
        }
    }

    void ApplyDrag()
    {
        Vector3 velocity = rb.linearVelocity;

        // Drag force proportional to the velocity
        Vector3 drag = -velocity * dragCoefficient * Time.fixedDeltaTime;
        rb.AddForce(drag, ForceMode.VelocityChange);
    }
}
