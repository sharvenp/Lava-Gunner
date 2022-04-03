using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    public float rotSpeed;
    void Update()
    {
        transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);   
    }
}
