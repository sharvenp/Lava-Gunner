using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    ObjectPooler objectPooler;
    Vector3 prevPos;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        //objectPooler.SpawnFromPool("Cube", new Vector3(0, previousy,0));
    }

    private void FixedUpdate()
    {
        SpawnCubes();
    }

    void SpawnCubes()
    {


    }

}
