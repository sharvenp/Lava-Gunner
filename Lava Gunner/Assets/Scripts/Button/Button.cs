using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameStateManager gameManager;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
            // win the game
            gameManager.WinGame();
		}
	}
}
