using UnityEngine;

public class ClayTargetSpawner : MonoBehaviour
{
    public GameObject clayTargetPrefab; // The prefab of the clay target
    public float spawnInterval = 2f; // Time interval between spawns
    public Transform spawnPoint; // The point from where the targets are spawned

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnClayTarget();
            timer = 0f;
        }
    }

    void SpawnClayTarget()
    {
        Instantiate(clayTargetPrefab, spawnPoint.position, Quaternion.identity);
    }
}
