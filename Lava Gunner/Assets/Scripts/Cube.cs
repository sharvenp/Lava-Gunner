using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float fallSpeed;

	private GameStateManager gameManager;

	// Start is called before the first frame update
	void Start()
    {
        fallSpeed = 5f;
		gameManager = FindObjectOfType<GameStateManager>();
	}

    // Update is called once per frame
    void LateUpdate()
    {
		if (gameManager.gameState == GameStateManager.gameStates.running)
		{
			transform.position += Vector3.down * fallSpeed * Time.deltaTime;
			if (transform.position.y <= 0)
			{
				// remove any children before recycling cube
				if (transform.childCount > 0)
				{
					foreach (Transform t in transform)
					{
						t.parent = null;
					}
				}
				gameObject.SetActive(false);
			}
		}
    }

	// handle player being on top of this by parenting player to this object
	private void OnTriggerEnter(Collider other)
	{
		// check if this is the player
		if (!other.CompareTag("Player"))
		{
			return;
		}
		other.transform.parent = this.transform;
	}

	// when player leaves unparent this
	private void OnTriggerExit(Collider other)
	{
		// check if this is the player
		if (!other.CompareTag("Player"))
		{
			return;
		}
		other.transform.parent = null;
	}
}
