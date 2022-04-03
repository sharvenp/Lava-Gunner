using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public GameObject retryPanel;
    public AudioManager audioManager;
    public ObjectPooler pooler;

    public gameStates gameState = 0; // 0 - running, 1 - lost, 2 - win
    public int numWins;
    public float cubeFallSpeedWinDelta;

    public enum gameStates { running, lost, win }

	private void Awake()
	{
        gameState = 0;
        retryPanel.SetActive(gameState == gameStates.lost);
        numWins = 0;
        cubeFallSpeedWinDelta = 50f;

    }

	private void Start()
    {
        gameState = 0;
        retryPanel.SetActive(gameState == gameStates.lost);
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R) && gameState == gameStates.lost)
		{
            // TODO: restart the game
		}
        if (Input.GetKeyDown(KeyCode.B))
        {
            WinGame();
        }
    }

	public void LoseGame()
    {
        gameState = gameStates.lost;
        numWins = 0;

        retryPanel.SetActive(true);
    }

    public void WinGame()
	{
        //gameState = gameStates.win;
        numWins += 1;
        audioManager.PlayBGM(numWins);
        foreach (GameObject obj in pooler.poolDictionary["Cube"])
        {
            Cube cube = obj.GetComponent<Cube>();
            cube.fallSpeed += cubeFallSpeedWinDelta;
        }
	}
}
