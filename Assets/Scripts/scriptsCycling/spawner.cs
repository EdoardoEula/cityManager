using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;  // Il prefab dell'oggetto che vuoi spawnare
    public float minSpawnInterval = 1f;  // Tempo minimo tra ogni spawn
    public float maxSpawnInterval = 3f;  // Tempo massimo tra ogni spawn
    public float minXPosition = -3.3f;    // Posizione minima lungo l'asse X
    public float maxXPosition = 3.3f;     // Posizione massima lungo l'asse X
    public float minYPosition = -2f;    // Posizione minima lungo l'asse Y
    public float maxYPosition = 2f;     // Posizione massima lungo l'asse Y

    private void Start()
    {
        // Inizia la coroutine per lo spawning casuale
        StartCoroutine(SpawnObjectsRandomly());
    }

    IEnumerator SpawnObjectsRandomly()
    {
        while (true)
        {
            SpawnObject();  // Chiama il metodo per spawnare l'oggetto
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);  // Aspetta un intervallo casuale prima del prossimo spawn
        }
    }

    void SpawnObject()
    {
        // Calcola posizione randomica all'interno dei limiti specificati
        float randomX = Random.Range(minXPosition, maxXPosition);
        float randomY = Random.Range(minYPosition, maxYPosition);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

        // Istanzia l'oggetto alla posizione randomica
        Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
    }
}