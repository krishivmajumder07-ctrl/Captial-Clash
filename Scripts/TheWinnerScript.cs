using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq; // Added for easy sorting

public class WinnerSummary : MonoBehaviour
{
    public TMP_Text WinnerText;
    public TMP_Text LeaderboardText;
    public Button RestartButton;

    void Start()
    {
        if (GameData.Instance == null) return;

        DisplayResults();

        RestartButton.onClick.AddListener(() => {
            GameData.Instance.ResetAll();
            SceneManager.LoadScene("Menu");
        });
    }

    void DisplayResults()
    {
        int count = GameData.Instance.PlayerCount;
        string results = "";
        
        // Find the index of the highest score
        int highestScore = -1;
        int winnerIndex = 0;

        for (int i = 0; i < count; i++)
        {
            results += $"{GameData.Instance.PlayerNames[i]} ({GameData.Instance.PlayerRoles[i]})\n";
            results += $"Score: {GameData.Instance.Scores[i]} | Budget: ₹{GameData.Instance.Budgets[i]:N0}\n\n";

            if (GameData.Instance.Scores[i] > highestScore)
            {
                highestScore = GameData.Instance.Scores[i];
                winnerIndex = i;
            }
        }

        WinnerText.text = $"🏆 WINNER: {GameData.Instance.PlayerNames[winnerIndex]}! 🏆";
        LeaderboardText.text = results;
    }
}
