using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public GameObject retryPanel;
    public AudioManager audioManager;
    public ObjectPooler pooler;

    private int gameState = 0; // 0 - running, 1 - lost, 2 - win
    public int numWins;
    public float cubeFallSpeedWinDelta;

	private void Awake()
	{
        gameState = 0;
        retryPanel.SetActive(gameState == 1);
        numWins = 0;
        cubeFallSpeedWinDelta = 10f;

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
        foreach (GameObject obj in pooler.poolDictionary["Cube"])
        {
            Cube cube = obj.GetComponent<Cube>();
            cube.fallSpeed += cubeFallSpeedWinDelta;
        }
	}
}
