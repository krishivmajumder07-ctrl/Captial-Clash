using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayNavigation : MonoBehaviour
{
    // Attach this to your "BACK" button
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Attach this to your "START CLASHING" button
    public void StartGame()
    {
        // This skips the menu and goes straight to player setup
        SceneManager.LoadScene("Setup");
    }
}
