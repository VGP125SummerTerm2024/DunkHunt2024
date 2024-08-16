using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] public GameObject[] DuckPrefabs;
    [SerializeField] public Transform[] SpawnPositions;
    [SerializeField] RoundManager rm;

    public int gameNum;

    public int rndNum;

    IEnumerator SpawnDuck2(float waitTime, int spawnPos)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject Duck2 = Instantiate(DuckPrefabs[Random.Range(0, DuckPrefabs.Length)], SpawnPositions[spawnPos].position, Quaternion.identity);

        // update the round manager and give it the duck and ducks name
        Duck2.GetComponent<DuckScript>().duckName = "Duck2";
        Duck2.GetComponent<DuckScript>().speedMult = rm.duckSpeedMult;
        rm.ducks++; ;
        rm.duckObj.Add(Duck2.GetComponent<DuckScript>().duckName, Duck2);

        Duck2.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 5);
    }

    private void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            DuckSpawner();
        }
        gameNum = GameManager.Instance.GameMode;
    }

    public void DuckSpawner()
    {
        //Select spawn positions for the ducks and ensure they are different
        int spawnPos1 = Random.Range(0, SpawnPositions.Length);
        int spawnPos2 = Random.Range(0, SpawnPositions.Length);
        GameObject Duck1;

        while (spawnPos2 == spawnPos1)
        {
            spawnPos2 = Random.Range(0, SpawnPositions.Length);
        }

        //instantiate the prefabs
        if (rndNum == 1)
        {
            Duck1 = Instantiate(DuckPrefabs[Random.Range(0, DuckPrefabs.Length - 1)], SpawnPositions[spawnPos1].position, Quaternion.identity);
        }
        else
        {
            Duck1 = Instantiate(DuckPrefabs[Random.Range(0, DuckPrefabs.Length)], SpawnPositions[spawnPos1].position, Quaternion.identity);
        }

        // update round manager and give it the duck and ducks name
        if (rm.firstRound)
            rm.firstRound = false;
        Duck1.GetComponent<DuckScript>().duckName = "Duck1";
        Duck1.GetComponent<DuckScript>().speedMult = rm.duckSpeedMult;
        rm.ducks++;
        rm.duckObj.Add(Duck1.GetComponent<DuckScript>().duckName, Duck1);

        if (gameNum == 2)
        {
            StartCoroutine(SpawnDuck2(0.75f, spawnPos2));
        }
    }
}
