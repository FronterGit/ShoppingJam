using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    public void startGame() {
        SceneManager.LoadScene("SampleScene");
    }

    public void openSettings() {
    }

    public void exit() {
#if UNITY_EDITOR
        // Stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}