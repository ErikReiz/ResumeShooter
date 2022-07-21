using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ResumeShooter.UI
{
    public class MainMenu : MonoBehaviour
    {
        #region SERIALIZE FIELDS
        [SerializeField] private Button playGameButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button quitGameButton;
		#endregion

		private void OnEnable()
		{
			playGameButton.onClick.AddListener(PlayGame);
			quitGameButton.onClick.AddListener(QuitGame);

			Cursor.lockState = CursorLockMode.Confined;
		}

		private void OnDisable()
		{
			playGameButton.onClick.RemoveListener(PlayGame);
			quitGameButton.onClick.RemoveListener(QuitGame);

			Cursor.lockState = CursorLockMode.None;
		}

		private void PlayGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		private void QuitGame()
		{
			Application.Quit();
		}

		public void Initialize(UnityAction settingsButtonDelegate)
		{
			settingsButton.onClick.RemoveAllListeners();
			settingsButton.onClick.AddListener(settingsButtonDelegate);
		}
	}
}