using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    ObjectPooler objectPooler;
    public GameStateManager gameStateManager;
    float prevY = 0.0f;

    public float minHeight = 0.0f;
    public float maxHeight = 470.0f;
    public float spawnDelay = 0.01f;
    public float xRange = 40f;
    public float yRange = 40f;

    private float deltaHeight;
    private float numOfObjects;
    private float nY;

    private void Spawn(float height)
    {
        float randomPosX = Random.Range(-xRange, xRange);
        float randomPosZ = Random.Range(-yRange, yRange);
        objectPooler.SpawnFromPool("Cube", new Vector3(randomPosX, height, randomPosZ));
    }
    IEnumerator SpawnCube()
    {
        while (gameStateManager.gameState == GameStateManager.gameStates.running)
        {
            Spawn(470);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        numOfObjects = objectPooler.pools[0].size;
        deltaHeight = maxHeight - minHeight;
        nY = deltaHeight / numOfObjects;

        for (int i = 0; i < objectPooler.pools[0].size; i++)
        {
            Spawn(prevY);
            prevY += nY;
        }

        StartCoroutine(SpawnCube());
    }

}
