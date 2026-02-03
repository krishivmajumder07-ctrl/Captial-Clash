using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoleSelectController : MonoBehaviour
{
    [Header("Assign Rows from Hierarchy")]
    public GameObject Row1;
    public GameObject Row2;
    public GameObject Row3;

    [Header("Assign the Start Button")]
    public Button StartGameButton;

    [Header("Feedback (Optional)")]
    public TMP_Text WarningText; // Drag a text object here to show "Duplicate Roles!"

    void Start()
    {
        if (GameData.Instance == null)
        {
            Debug.LogError("GameData missing! Please start from the Menu scene.");
            return;
        }

        if (StartGameButton != null)
        {
            StartGameButton.onClick.RemoveAllListeners();
            StartGameButton.onClick.AddListener(StartMainGame);
        }

        SetupRow(Row1, 0);
        SetupRow(Row2, 1);

        if (GameData.Instance.PlayerCount < 3)
        {
            if (Row3 != null) Row3.SetActive(false);
        }
        else
        {
            if (Row3 != null)
            {
                Row3.SetActive(true);
                SetupRow(Row3, 2);
            }
        }
    }

    void SetupRow(GameObject row, int index)
    {
        if (row == null) return;
        TMP_Text nameLabel = row.GetComponentInChildren<TMP_Text>();
        if (nameLabel != null && index < GameData.Instance.PlayerNames.Count)
        {
            nameLabel.text = GameData.Instance.PlayerNames[index];
        }
    }

    public void StartMainGame()
    {
        // 1. Get Dropdowns
        TMP_Dropdown d1 = Row1.GetComponentInChildren<TMP_Dropdown>();
        TMP_Dropdown d2 = Row2.GetComponentInChildren<TMP_Dropdown>();
        TMP_Dropdown d3 = (Row3 != null) ? Row3.GetComponentInChildren<TMP_Dropdown>() : null;

        // 2. Get Selected Values
        string role1 = d1.options[d1.value].text;
        string role2 = d2.options[d2.value].text;
        string role3 = (d3 != null) ? d3.options[d3.value].text : "";

        // 3. Duplicate Role Check
        bool isDuplicate = false;
        if (role1 == role2) isDuplicate = true;
        if (GameData.Instance.PlayerCount == 3)
        {
            if (role1 == role3 || role2 == role3) isDuplicate = true;
        }

        if (isDuplicate)
        {
            if (WarningText != null) WarningText.text = "Error: Each player must choose a unique role!";
            Debug.LogWarning("Duplicate roles detected!");
            return; // STOP the game from starting
        }

        // 4. Save and Proceed
        GameData.Instance.PlayerRoles[0] = role1;
        GameData.Instance.PlayerRoles[1] = role2;
        if (GameData.Instance.PlayerCount == 3) GameData.Instance.PlayerRoles[2] = role3;

        GameData.Instance.CurrentPlayerIndex = 0;
        SceneManager.LoadScene("GameMain");
    }
}
