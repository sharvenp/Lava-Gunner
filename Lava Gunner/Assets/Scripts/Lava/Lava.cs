using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float lavaSpeed;
    public Vector3 startingPos;
    public Vector3 endPos;

    public GameStateManager gameManager;

    private bool isMoving;

    private void Start()
    {
        isMoving = true;
        transform.position = startingPos;
    }

    private void Update()
    {
        if (isMoving)
            transform.position += Vector3.up * lavaSpeed * Time.deltaTime;
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
            // end the game
            gameManager.LoseGame();
            isMoving = false;
		}
	}
}
