using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    ObjectPooler objectPooler;
    float prevY = 0.0f;

    public float minHeight = 0.0f;
    public float maxHeight = 470.0f;

    private float deltaHeight;
    private float numOfObjects;
    private float nY;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        numOfObjects = objectPooler.pools[0].size;
        deltaHeight = maxHeight - minHeight;
        nY = deltaHeight / numOfObjects;

        for (int i = 0; i < objectPooler.pools[0].size; i++)
        {
            float randomPosX = Random.Range(-40, 40);
            float randomPosZ = Random.Range(-40, 40);
            objectPooler.SpawnFromPool("Cube", new Vector3(randomPosX, prevY, randomPosZ));
            prevY += nY;
        }
    }

    private void Update()
    {

    }



}
