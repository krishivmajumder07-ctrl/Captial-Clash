using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        // 1. Check if GameData exists before resetting
        if (GameData.Instance != null)
        {
            GameData.Instance.ResetAll();
        }
        else
        {
            Debug.LogWarning("GameData instance not found. Continuing to Setup...");
        }

        // 2. Load the Setup scene
        SceneManager.LoadScene("Setup");
    }

    // --- NEW METHOD FOR HOW TO PLAY ---
    public void OpenHowToPlay()
    {
        // Ensure the scene name "HowToPlay" matches your scene file exactly
        SceneManager.LoadScene("HowToPlay");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game Requested");
        Application.Quit();
    }
}
