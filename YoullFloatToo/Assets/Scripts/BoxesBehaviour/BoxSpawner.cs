using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class RandomSpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject objectToSpawn; // Prefab del objeto a spawnear
    [SerializeField] private Transform[] spawnPoints;  // Array de puntos de spawn
    [SerializeField] private float spawnInterval = 2.5f; // Intervalo en segundos entre cada spawn

    private Coroutine spawnCoroutine;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // Solo el servidor debe manejar el spawn
        {
            StartSpawning();
        }
    }

    private void StartSpawning()
    {
        spawnCoroutine = StartCoroutine(SpawnObjectsCoroutine());
    }

    private IEnumerator SpawnObjectsCoroutine()
    {
        while (true) // Ciclo infinito para spawnear objetos periódicamente
        {
            SpawnRandomObject();
            yield return new WaitForSeconds(spawnInterval); // Esperar el intervalo antes del próximo spawn
        }
    }

    private void SpawnRandomObject()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length); // Escoger un punto aleatorio
        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawnear el objeto en el punto seleccionado
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation, gameObject.transform);
        spawnedObject.GetComponent<NetworkObject>().Spawn(); // Sincronizar con todos los clientes
    }

    public override void OnDestroy()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine); // Detener la corutina si el objeto se destruye
        }
        base.OnDestroy();
    }
}
