using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
	public float xSensitivity = 100f;
	public float ySensitivity = 100f;

	public Transform player;
	private float x = 0f;

    // Start is called before the first frame update
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
		float mouseX = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

		// mouse rotation
		x -= mouseY;
		x = Mathf.Clamp(x, -80, 80);
		transform.localRotation = Quaternion.Euler(x, 0f, 0f);

		player.Rotate(Vector3.up * mouseX);
	}
}
