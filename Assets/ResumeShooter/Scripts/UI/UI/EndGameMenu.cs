using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ResumeShooter.UI
{

	public class EndGameMenu : MonoBehaviour
	{
		#region SERIALIZE FIELDS
		[SerializeField] private Button restartGameButton;
		[SerializeField] private Button quitGameButton;
		#endregion

		private void OnEnable()
		{
			restartGameButton.onClick.AddListener(RestartGame);
			quitGameButton.onClick.AddListener(QuitGame);

			Cursor.lockState = CursorLockMode.Confined;
		}

		private void OnDisable()
		{
			restartGameButton.onClick.RemoveListener(RestartGame);
			quitGameButton.onClick.RemoveListener(QuitGame);

			Cursor.lockState = CursorLockMode.Locked;
		}

		private void RestartGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			Time.timeScale = 1f;
		}

		private void QuitGame()
		{
			Application.Quit();
		}
	}
}