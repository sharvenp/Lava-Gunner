using UnityEngine;

public class GrapplingGun : MonoBehaviour {

    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cam, player;
    public float maxDistance = 100f;
    private SpringJoint joint;

	public float spring;
	public float damper;
	public float mass;
    public float fireRate;

	public float minDist;
	public float maxDist;

    Animator gunAnimator;
    public AudioSource gunAudioSource;
    public AudioClip gunSound;
    public ParticleSystem muzzleFlash;

    private GameStateManager gameManager;

    private float nextTimeToFire = 0f;

	private void Start()
	{
        gunAnimator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameStateManager>();
    }

    void Update() {

        if (gameManager.gameState == GameStateManager.gameStates.running)
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                StartGrapple();
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopGrapple();
            }
        }
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    public void StartGrapple() {

        gunAnimator.Play("Shoot", 0, 0);
        gunAudioSource.PlayOneShot(gunSound);
        muzzleFlash.Play();

		if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDistance, whatIsGrappleable))
		{
			grapplePoint = hit.point;
			joint = player.gameObject.AddComponent<SpringJoint>();
			joint.autoConfigureConnectedAnchor = false;
			joint.connectedAnchor = grapplePoint;

			float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

			//The distance grapple will try to keep from grapple point. 
			joint.maxDistance = distanceFromPoint * maxDist;
			joint.minDistance = distanceFromPoint * minDist;

			//Adjust these values to fit your game.
			joint.spring = spring;
			joint.damper = damper;
			joint.massScale = mass;
		}
	}


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    public void StopGrapple() {
        Destroy(joint);
    }
    
    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
