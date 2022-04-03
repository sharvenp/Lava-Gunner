using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float fallSpeed;
    // Start is called before the first frame update
    void Start()
    {
        fallSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }
}
