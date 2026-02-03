using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [Header("Game Settings")]
    public int PlayerCount;
    public int StartingCapital;
    public int TotalRounds = 5;

    [Header("Live Game State")]
    public List<string> PlayerNames = new List<string>();
    public List<string> PlayerRoles = new List<string>();
    public List<int> Budgets = new List<int>();
    public List<int> Scores = new List<int>();
    public List<bool> ConfirmedThisRound = new List<bool>();

    public int CurrentPlayerIndex = 0;
    public int CurrentRound = 1;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void InitializeGame(int count, List<string> names, int capital)
    {
        PlayerCount = count;
        PlayerNames = new List<string>(names);
        StartingCapital = capital;

        Budgets.Clear(); Scores.Clear(); PlayerRoles.Clear(); ConfirmedThisRound.Clear();

        for (int i = 0; i < count; i++)
        {
            Budgets.Add(StartingCapital);
            Scores.Add(0);
            PlayerRoles.Add("None");
            ConfirmedThisRound.Add(false);
        }
    }

    public void ResetTurnConfirmations()
    {
        for (int i = 0; i < ConfirmedThisRound.Count; i++) ConfirmedThisRound[i] = false;
    }

    public void ResetAll()
    {
        PlayerNames.Clear(); PlayerRoles.Clear(); Budgets.Clear();
        Scores.Clear(); ConfirmedThisRound.Clear();
        CurrentPlayerIndex = 0; CurrentRound = 1;
    }
}
