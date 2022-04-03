using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	public void GoToScene(int i)
	{
		SceneManager.LoadScene(i);
	}
}
