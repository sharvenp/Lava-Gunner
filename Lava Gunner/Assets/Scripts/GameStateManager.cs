using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public GameObject retryPanel;
    public AudioManager audioManager;

    private int gameState = 0; // 0 - running, 1 - lost, 2 - win
    public int numWins;

	private void Awake()
	{
        gameState = 0;
        retryPanel.SetActive(gameState == 1);
        numWins = 0;
	}

	private void Start()
    {
        gameState = 0;
        retryPanel.SetActive(gameState == 1);
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R) && gameState == 1)
		{
            // TODO: restart the game
		}
	}

	public void LoseGame()
    {
        gameState = 1;
        numWins = 0;

        retryPanel.SetActive(true);
    }

    public void WinGame()
	{
        gameState = 2;
        numWins += 1;
        audioManager.PlayBGM(numWins);
	}
}
