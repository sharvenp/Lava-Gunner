using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;
	public float jumpForce = 5f;
	public float gravity = 10f;
	public Transform feet;
	public LayerMask floorMask;
	public float floorDist;

	private CharacterController ctrl;
	private Vector3 velocity;
	[SerializeField] private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
		ctrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		isGrounded = Physics.CheckSphere(feet.position, floorDist, floorMask);

		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		Vector3 input = transform.right * x + transform.forward * z;
		input = input.normalized;
		ctrl.Move(input * speed * Time.deltaTime);

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			print("jump");
			velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
		}

		velocity.y -= gravity * Time.deltaTime;
		ctrl.Move(velocity * Time.deltaTime);

	}
}
