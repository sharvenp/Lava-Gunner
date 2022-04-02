using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementContoller: MonoBehaviour
{
	[Header("Movement")]
	public float speed = 6f;
	public float jumpForce = 5f;
	public float gravity = 10f;
	public Transform feet;
	public LayerMask floorMask;
	public float floorDist;

	[Space]

	[Header("Grappling")]
	public LayerMask surface;
	public Transform tip;
	public LineRenderer lr;

	public Transform gunTip, cam, player;
	private float maxDistance = 100f;
	private SpringJoint joint;
	private Vector3 grapplePoint;

	private CharacterController ctrl;
	private Vector3 velocity;
	[SerializeField] private bool isGrounded = false;
	[SerializeField] private bool isGrappling = false;

	private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
		ctrl = GetComponent<CharacterController>();
		rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			isGrappling = true;
			ctrl.enabled = false;
			StartGrapple();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			isGrappling = false;
			ctrl.enabled = true;
			StopGrapple();
		}
		if (ctrl.enabled)
		{
			ControllerMove();
		}
	}

	private void ControllerMove()
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
			velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
		}

		velocity.y -= gravity * Time.deltaTime;
		ctrl.Move(velocity * Time.deltaTime);

	}

	void StartGrapple()
	{
		if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDistance, surface))
		{
			grapplePoint = hit.point;
			joint = player.gameObject.AddComponent<SpringJoint>();
			joint.autoConfigureConnectedAnchor = false;
			joint.connectedAnchor = grapplePoint;

			float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

			//The distance grapple will try to keep from grapple point. 
			joint.maxDistance = distanceFromPoint * 0.3f;
			joint.minDistance = distanceFromPoint * 0.25f;

			//Adjust these values to fit your game.
			joint.spring = 45f;
			joint.damper = 1f;
			joint.massScale = 4.5f;

			lr.positionCount = 2;
			currentGrapplePosition = gunTip.position;
		}
	}
	
	void StopGrapple()
	{
		lr.positionCount = 0;
		Destroy(joint);
	}

	private Vector3 currentGrapplePosition;

	void DrawRope()
	{
		//If not grappling, don't draw rope
		if (!joint) return;

		currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

		lr.SetPosition(0, gunTip.position);
		lr.SetPosition(1, currentGrapplePosition);
	}

	public bool IsGrappling()
	{
		return joint != null;
	}

	public Vector3 GetGrapplePoint()
	{
		return grapplePoint;
	}
}
/*
 * using UnityEngine;

public class GrapplingGun : MonoBehaviour {

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple() {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
 * 
 */
