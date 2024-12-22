using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterLevel : MonoBehaviour
{
    public static WaterLevel Instance { get; private set; }

    // Public variables for wave control
    [SerializeField] float waveAmplitude = 1f; // Amplitude of the wave
    [SerializeField] float waveFrequency = 1f; // Frequency of the wave
    [SerializeField] float waveSpeed = 1f;     // Speed of the wave
    [SerializeField] Vector2 waveDirection = new Vector2(1, 0); // Direction of the wave

    Vector3 startingPos;
    float time;

    public float currentWaterLevel;
    MeshFilter meshFilter;
    Vector3[] originalVertices;
    Vector3[] displacedVertices;

    void Awake()
    {
        startingPos = transform.position;

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Cache the mesh and its vertices
        meshFilter = GetComponent<MeshFilter>();
        originalVertices = meshFilter.mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        time += Time.deltaTime * waveSpeed;
        SimulateTide();
        ModifyMeshVertices();
    }

    void SimulateTide()
    {
        // Simulate the general water level rising and falling (tide)
        float tideOffset = Mathf.Sin(time * waveFrequency) * waveAmplitude;
        transform.position = startingPos + new Vector3(0, tideOffset, 0);

        // Update current water level
        currentWaterLevel = transform.position.y;
    }

    void ModifyMeshVertices()
    {
        // Access and modify each vertex for wave displacement
        Mesh mesh = meshFilter.mesh;

        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];

            // World position of the vertex
            Vector3 worldPosition = transform.TransformPoint(vertex);

            // Apply wave displacement using sine wave formula
            float wave = Mathf.Sin((worldPosition.x * waveDirection.x + worldPosition.z * waveDirection.y) * waveFrequency + time) * waveAmplitude;
            displacedVertices[i] = vertex + new Vector3(0, wave, 0);
        }

        // Update the mesh vertices
        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
    }
}
