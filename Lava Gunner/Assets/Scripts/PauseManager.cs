using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

	private bool isPaused = false;

	private void Awake()
	{
		isPaused = false;
		Time.timeScale = 0f;
		pausePanel.gameObject.SetActive(isPaused);
	}

	private void Start()
	{
		isPaused = false;
		Time.timeScale = 1f;
		pausePanel.gameObject.SetActive(isPaused);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		isPaused = !isPaused;
		pausePanel.gameObject.SetActive(isPaused);
		if (isPaused)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void GoToMainMenu()
	{
		// Go back to main menu (assuming it is scene 0)
		SceneManager.LoadScene(0);
	}

	public void ExitGame()
	{
		Debug.Log("Exiting game.");
		Application.Quit();
	}
}
