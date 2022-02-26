using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] grobblinPrefabs;


    public float timeBetweenSpawn;
    public float timeBetweenGrobblins;
    public float randomTimeAdjust;
    public float timer;
    float grobblinTimer;


    // Start is called before the first frame update
    void Start()
    {
        timer = timeBetweenSpawn + Random.Range(-randomTimeAdjust, randomTimeAdjust);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        timer -= deltaTime;
        grobblinTimer -= deltaTime;
        if (timer <= 0)
        {
            timer = timeBetweenSpawn + Random.Range(-randomTimeAdjust, randomTimeAdjust);
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform);
        }
        if (grobblinTimer <= 0)
        {
            grobblinTimer = timeBetweenGrobblins + Random.Range(-randomTimeAdjust, randomTimeAdjust);
            Instantiate(grobblinPrefabs[Random.Range(0, grobblinPrefabs.Length)]);

        }
    }
}
