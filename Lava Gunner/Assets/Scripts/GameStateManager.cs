using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject player;
    public Vector3 playerStartPos;
    public GameObject lava;

    public GameObject retryPanel;
    public GameObject msgPanel;
    public AudioManager audioManager;
    public ObjectPooler pooler;

    public TextMeshProUGUI levelText;

    public gameStates gameState = 0; // 0 - running, 1 - lost, 2 - win
    public int numWins;
    public float cubeFallSpeedWinDelta;
    public float lavaRiseSpeedDelta;

    public enum gameStates { running, lost, win }

    private ScoreManager scoreManager;

    private void Awake()
	{
        gameState = gameStates.running;
        retryPanel.SetActive(false);
        msgPanel.SetActive(false);
        numWins = 0;
    }

	private void Start()
    {
        gameState = gameStates.running;
        retryPanel.SetActive(false);
        msgPanel.SetActive(false);
        numWins = 0;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R) && gameState == gameStates.lost)
		{
            // reload the scene
            SceneManager.LoadScene(1);
		}
        /*if (Input.GetKeyDown(KeyCode.B))
        {
            WinGame();
        }*/
    }

	public void LoseGame()
    {
        gameState = gameStates.lost;
        numWins = 0;

        retryPanel.SetActive(true);
        scoreManager.StopTimer();
    }

    public void WinGame()
	{
        msgPanel.SetActive(true);
        msgPanel.GetComponent<Animator>().Play("FlashMessage", 0, 0f);

        //gameState = gameStates.win;
        numWins += 1;
        audioManager.PlayBGM(numWins);
        foreach (GameObject obj in pooler.poolDictionary["Cube"])
        {
            Cube cube = obj.GetComponent<Cube>();
            cube.fallSpeed += cubeFallSpeedWinDelta;
        }

        levelText.text = $"LVL {numWins}";

        // reset the lava
        lava.transform.position = lava.GetComponent<Lava>().startingPos;
        lava.GetComponent<Lava>().lavaSpeed += lavaRiseSpeedDelta;

        // reset the player
        player.transform.position = playerStartPos;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerMovement>().grapplingGun.StopGrapple();

        scoreManager.StopTimer();
        scoreManager.StartTimer();
    }
}
