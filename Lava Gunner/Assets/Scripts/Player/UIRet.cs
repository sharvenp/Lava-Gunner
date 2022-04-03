using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
public class UIRet : MonoBehaviour
{
	public Image crosshair;

	private PlayerMovement player;
	public GrapplingGun gun;
	private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
		cam = Camera.main;
		player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, 
			out RaycastHit hit, gun.maxDistance, gun.whatIsGrappleable))
		{
			crosshair.color = Color.red;
		}
		else
		{
			crosshair.color = Color.blue;
		}
	}
}
