using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;  // Il prefab dell'oggetto che vuoi spawnare
    public float spawnInterval = 2f;   // Intervallo fisso tra ogni spawn
    public float minXPosition = -3.3f;  // Posizione minima lungo l'asse X
    public float maxXPosition = 3.3f;   // Posizione massima lungo l'asse X
    public float minYPosition = -2f;    // Posizione minima lungo l'asse Y
    public float maxYPosition = 2f;     // Posizione massima lungo l'asse Y
    public float gameDuration = 30f;    // Durata massima del gioco in secondi

    private void Start()
    {
        // Inizia la coroutine per lo spawning periodico
        StartCoroutine(SpawnObjectsPeriodically());

        // Inizia la coroutine per fermare il gioco dopo la durata specificata
        StartCoroutine(StopGameAfterDuration());
    }

    IEnumerator SpawnObjectsPeriodically()
    {
        while (true)
        {
            SpawnObject();  // Chiama il metodo per spawnare l'oggetto
            yield return new WaitForSeconds(spawnInterval);  // Aspetta un intervallo fisso prima del prossimo spawn
        }
    }

    IEnumerator StopGameAfterDuration()
    {
        yield return new WaitForSeconds(gameDuration);
        // Add any logic to stop the game here
        Debug.Log("Game Over");
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(GameManager.currentUser);
        userRef.Child("moneyAvailable").SetValueAsync(GameManager.money_available);
    }

    void SpawnObject()
    {
        // Calcola posizione randomica all'interno dei limiti specificati
        float randomX = Random.Range(minXPosition, maxXPosition);
        float randomY = Random.Range(minYPosition, maxYPosition);

        // Clamp the random positions to ensure they are within the specified range
        randomX = Mathf.Clamp(randomX, minXPosition, maxXPosition);
        randomY = Mathf.Clamp(randomY, minYPosition, maxYPosition);

        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

        // Istanzia l'oggetto alla posizione randomica
        Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
    }
}
