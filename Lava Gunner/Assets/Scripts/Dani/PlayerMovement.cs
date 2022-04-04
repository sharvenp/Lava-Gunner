// Some stupid rigidbody based movement by Dani

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour {

    //Assingables
    public Transform playerCam;
    public Transform orientation;

	// ground detection for falling objects
	public Transform feet;
	public float groundThreshold = 0.15f;
	
    public GrapplingGun grapplingGun;

    //Audio
    public AudioSource windAudioSource;

    //Post-processing
    public Volume postProcessingVolume;
    private ChromaticAberration aberration;

    //Other
    private Rigidbody rb;

	public float maxMag = 10f;
    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    
    //Movement
    public float moveSpeed = 4500;
	public float flightMult = 2f;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;
	public LayerMask wall;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;
    
    //Input
    float x, y;
    bool jumping, sprinting, crouching;
    
    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    private GameStateManager gameManager;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start() {
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // start playing the wind sound
        windAudioSource.loop = true;
        windAudioSource.Play();

        postProcessingVolume.profile.TryGet(out aberration);

        gameManager = FindObjectOfType<GameStateManager>();
    }

    
    private void FixedUpdate() {

        if (gameManager.gameState == GameStateManager.gameStates.running)
		{
			MovePos();
			CheckGroundPhys();
		}
    }

    private void Update() {
        if (gameManager.gameState == GameStateManager.gameStates.running)
        {
            MyInput();
            Look();
        }
	}

	private void LateUpdate()
	{
        if (gameManager.gameState == GameStateManager.gameStates.running)
        {
            MovementEffects();
        }
	}

	private void MovePos()
	{
		//Vector3 newPos = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 forwardVector = orientation.transform.forward * y * Time.deltaTime;
		Vector3 tangentVector = orientation.transform.right * x * Time.deltaTime;

		Vector3 newPos = forwardVector + tangentVector;
		newPos = newPos.normalized * moveSpeed;

		if (!grounded)
		{
			newPos = newPos * flightMult;
		}
		
		//If holding jump && ready to jump, then jump
		if (readyToJump && jumping) Jump();

		// check if we are going to hit a wall
		if (Physics.Linecast(transform.position, transform.position + newPos, out RaycastHit hit, wall))
		{
			// we hit a wall
			return;
		}

		rb.MovePosition(transform.position + newPos);

	}

	/// <summary>
	/// Find user input. Should put this in its own class but im lazy
	/// </summary>
	private void MyInput() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

		// adds a speed limit
		if (rb.velocity.magnitude > maxMag)
		{
			rb.velocity = rb.velocity.normalized * maxMag;
		}
    }

    private void MovementEffects()
	{
        float normalizedVelocityMagnitude = 0f;
        if (!grounded)
		{
            normalizedVelocityMagnitude = Mathf.Clamp01(rb.velocity.magnitude / maxMag);
		}
        windAudioSource.volume = Mathf.Lerp(windAudioSource.volume, normalizedVelocityMagnitude, 0.05f);
        aberration.intensity.value = Mathf.Lerp(aberration.intensity.value, normalizedVelocityMagnitude, 0.05f);
    }

    private void Jump() {
        if (grounded && readyToJump) {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);
            
            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    private void ResetJump() {
        readyToJump = true;
    }
    
    private float desiredX;
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    
    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other) {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
    }

	private void CheckGroundPhys()
	{
		if (Physics.OverlapSphere(feet.position, groundThreshold, whatIsGround).Length > 0)
		{
			grounded = true;
		}
	}
}
