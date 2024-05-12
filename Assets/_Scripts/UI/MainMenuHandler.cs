using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    public GameObject settingsPanel;
    public GameObject menuPanel;
    public void startGame() {
        SceneManager.LoadScene("GAME");
    }

    public void openSettings() {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void exit() {
#if UNITY_EDITOR
        // Stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}