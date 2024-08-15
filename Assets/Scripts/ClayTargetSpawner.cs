using UnityEngine;
using System.Collections;

public class ClayTargetSpawner : MonoBehaviour
{
    public GameObject clayTargetPrefab; // The prefab of the clay target
    public float spawnInterval = 2f; // Time interval between spawns
    public float minX; // Minimum x value for spawning
    public float maxX; // Maximum x value for spawning
    public float spawnY; // The y value where the targets should be spawned
    public int maxTargets = 10; // Maximum number of targets to spawn
    public float breakInterval = 3f; // Time to wait before spawning the next set of targets
    public float minDelayBetweenTargets = 0.5f; // Minimum delay between the two targets
    public float maxDelayBetweenTargets = 1.5f; // Maximum delay between the two targets

    private int spawnedTargets = 0; // Counter to keep track of the number of spawned targets

    public AudioSource audioSource;
    public AudioClip spawnClip;
    public AmmoManager ammoManager;
   

    private void Start()
    {
        StartCoroutine(SpawnTargets());
    }

    private IEnumerator SpawnTargets()
    {
        while (spawnedTargets < maxTargets)
        {
            // Spawn the first target
            SpawnClayTarget();

            // Wait for a random delay between the two targets
            float randomDelay = UnityEngine.Random.Range(minDelayBetweenTargets, maxDelayBetweenTargets);
            yield return new WaitForSeconds(randomDelay);

            // Spawn the second target
            SpawnClayTarget();
            spawnedTargets += 2;

            // Wait for the break interval before spawning the next pair
            yield return new WaitForSeconds(breakInterval);

            ammoManager.reload();
        }
    }

    void SpawnClayTarget()
    {
        float randomX = UnityEngine.Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
        Instantiate(clayTargetPrefab, spawnPosition, Quaternion.identity);
        PlaySoundOnce(spawnClip);
        
    }

    private void PlaySoundOnce(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
