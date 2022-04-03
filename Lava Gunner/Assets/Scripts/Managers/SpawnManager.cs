using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float timeOfLastSpawn = 0f;
    public float timeToSpawn = 1.0f;
    ObjectPooler objectPooler;
    float prevY = 0.0f;
    public float deltaHeight = 0.5f;
    public float maxHeight = 470.0f;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        for (int i = 0; i < objectPooler.pools[0].size ; i++)
        {
            float randomPosX = Random.Range(-50, 50);
            float randomPosZ = Random.Range(-50, 50);
            objectPooler.SpawnFromPool("Cube", new Vector3(randomPosX, prevY, randomPosZ));
            prevY += deltaHeight;
        }
    }

    private void Update()
    {
        if ((Time.time - timeOfLastSpawn) >= timeToSpawn)
        {
            SpawnCubes();
        }
    }

    void SpawnCubes()
    {
        timeOfLastSpawn = Time.time;
        if (prevY <= maxHeight)  
        {
            float randomPosX = Random.Range(-50, 50);
            float randomPosZ = Random.Range(-50, 50);
            objectPooler.SpawnFromPool("Cube", new Vector3(randomPosX, prevY, randomPosZ));
            prevY += deltaHeight;
        }

    }

}
