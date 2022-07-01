using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Confined;
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void ReloadGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 1f;
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
