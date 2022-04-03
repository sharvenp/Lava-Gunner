using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    /*#region Singleton

     public static ObjectPooler Instance;

     private void Awake()
     {
         Instance = this;
     }

     #endregion*/


    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;

        }

        GameObject objectToSpawn = poolDictionary[tag].Find((GameObject obj) => !obj.activeInHierarchy);
        if (objectToSpawn == null)
        {
            return null;
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;

        return objectToSpawn;
    }

}