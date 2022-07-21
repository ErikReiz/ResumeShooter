using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ResumeShooter.UI
{

	public class EndGameMenu : MonoBehaviour
	{
		#region SERIALIZE FIELDS
		[SerializeField] private Button nextLevelButton;
		[SerializeField] private Button restartGameButton;
		[SerializeField] private Button quitToMenuButton;
		#endregion

		private void OnEnable()
		{
			nextLevelButton?.onClick.AddListener(LoadNextLevel);
			restartGameButton?.onClick.AddListener(RestartGame);
			quitToMenuButton?.onClick.AddListener(QuitToMainMenu);

			Cursor.lockState = CursorLockMode.Confined;
		}

		private void OnDisable()
		{
			nextLevelButton?.onClick.RemoveListener(LoadNextLevel);
			restartGameButton.onClick?.RemoveListener(RestartGame);
			quitToMenuButton.onClick?.RemoveListener(QuitToMainMenu);

			Cursor.lockState = CursorLockMode.None;
		}

		private void LoadNextLevel()
		{
			Time.timeScale = 1f;
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			if (currentSceneIndex >= SceneManager.sceneCount)
				currentSceneIndex = 0;
			else
				currentSceneIndex++;

			SceneManager.LoadScene(currentSceneIndex);
		}

		private void RestartGame()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void QuitToMainMenu()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(0);
		}
	}
}