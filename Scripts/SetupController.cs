using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SetupController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField PlayerCountInput;
    public TMP_Dropdown CapitalDropdown;
    public GameObject NameInputPrefab;
    public Transform NamesContainer;

    private List<TMP_InputField> nameFields = new List<TMP_InputField>();

    public void GeneratePlayers()
    {
        // Clear existing fields
        foreach (Transform child in NamesContainer) {
            Destroy(child.gameObject);
        }
        nameFields.Clear();

        if (int.TryParse(PlayerCountInput.text, out int count))
        {
            // Lock to 2-3 players as per your game design
            count = Mathf.Clamp(count, 2, 3);
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(NameInputPrefab, NamesContainer);
                TMP_InputField input = obj.GetComponent<TMP_InputField>();
                if (input != null)
                {
                    input.placeholder.GetComponent<TMP_Text>().text = $"Enter Player {i + 1} Name...";
                    nameFields.Add(input);
                }
            }
        }
    }

    public void ContinueToRoleSelect()
    {
        // 1. Check if GameData actually exists
        if (GameData.Instance == null)
        {
            Debug.LogError("FATAL: GameData instance is missing! Make sure you started from the Menu scene.");
            return;
        }

        // 2. Gather names
        List<string> names = new List<string>();
        for (int i = 0; i < nameFields.Count; i++)
        {
            string pName = string.IsNullOrEmpty(nameFields[i].text) ? $"Player {i + 1}" : nameFields[i].text;
            names.Add(pName);
        }

        // 3. Clean and Parse Capital (Regex fix for commas/INR)
        string dropdownText = CapitalDropdown.options[CapitalDropdown.value].text;
        string cleanNumber = Regex.Replace(dropdownText, "[^0-9]", "");

        if (int.TryParse(cleanNumber, out int capital))
        {
            GameData.Instance.InitializeGame(names.Count, names, capital);
            SceneManager.LoadScene("RoleSelect");
        }
        else
        {
            Debug.LogError($"Format Error: Could not extract numbers from '{dropdownText}'");
        }
    }
}
