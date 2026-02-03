using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameMainController : MonoBehaviour
{
    [Header("Separate HUD Labels")]
    public TMP_Text RoundText;
    public TMP_Text TurnText;
    public TMP_Text RoleText;
    public TMP_Text BudgetText;
    public TMP_Text MessageText;

    [Header("Investment Sliders")]
    public Slider QualitySlider;
    public Slider LocationSlider;
    public Slider StaffSlider;

    [Header("Slider Value Texts")]
    public TMP_Text QualityValueText;
    public TMP_Text LocationValueText;
    public TMP_Text StaffValueText;

    [Header("Buttons")]
    public Button ConfirmButton;
    public Button NextButton;

    private string[] positiveEvents = { "Viral Social Media Post!", "Local Government Grant received!", "Unexpected Tax Refund!", "Celebrity Endorsement!" };
    private string[] negativeEvents = { "Unexpected Maintenance Costs!", "Supply Chain Disruption!", "Inflation Spike!", "New Competitor nearby!" };

    void Start()
    {
        if (GameData.Instance == null) return;

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(OnConfirmInvestment);
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(OnNextTurn);

        QualitySlider.onValueChanged.AddListener(delegate { UpdateSliderLabels(); });
        LocationSlider.onValueChanged.AddListener(delegate { UpdateSliderLabels(); });
        StaffSlider.onValueChanged.AddListener(delegate { UpdateSliderLabels(); });

        RefreshUI();
    }

    // --- NEW: MANUAL OVERRIDE FOR HACKATHON ---
    void Update()
    {
        // If the game gets stuck or you need to show the winner quickly:
        // Press 'K' on your keyboard to force the GameOver scene.
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Manual Override: Loading GameOver Scene...");
            SceneManager.LoadScene("GameOver");
        }
    }

    void UpdateSliderLabels()
    {
        if (QualityValueText) QualityValueText.text = QualitySlider.value.ToString();
        if (LocationValueText) LocationValueText.text = LocationSlider.value.ToString();
        if (StaffValueText) StaffValueText.text = StaffSlider.value.ToString();
    }

    void RefreshUI()
    {
        GameData gd = GameData.Instance;
        int idx = gd.CurrentPlayerIndex;

        if (RoundText) RoundText.text = $"Round: {gd.CurrentRound} / {gd.TotalRounds}";
        if (TurnText) TurnText.text = $"Turn: {gd.PlayerNames[idx]}";
        if (RoleText) RoleText.text = $"Role: {gd.PlayerRoles[idx]}";
        if (BudgetText) BudgetText.text = $"Remaining: ₹{gd.Budgets[idx]:N0}";

        bool alreadyConfirmed = gd.ConfirmedThisRound[idx];
        ConfirmButton.interactable = !alreadyConfirmed;
        NextButton.interactable = alreadyConfirmed;

        if (!alreadyConfirmed)
        {
            MessageText.text = "Plan your investment and hit Confirm!";
            QualitySlider.value = 0;
            LocationSlider.value = 0;
            StaffSlider.value = 0;
        }
    }

    public void OnConfirmInvestment()
    {
        GameData gd = GameData.Instance;
        int idx = gd.CurrentPlayerIndex;

        int cost = (int)(QualitySlider.value + LocationSlider.value + StaffSlider.value) * 10000;

        if (gd.Budgets[idx] >= cost)
        {
            gd.Budgets[idx] -= cost;
            int pointsGained = (int)(QualitySlider.value * 2 + LocationSlider.value * 1.5f + StaffSlider.value);
            gd.Scores[idx] += pointsGained;

            string eventMsg = "";
            if (Random.value < 0.3f)
            {
                eventMsg = TriggerRandomEvent(idx);
            }

            MessageText.text = $"{eventMsg}\n<color=yellow>Investment Success!</color> Score +{pointsGained}. Total Score: {gd.Scores[idx]}";
            gd.ConfirmedThisRound[idx] = true;
            RefreshUI();
        }
        else
        {
            MessageText.text = "<color=red>Insufficient Budget!</color>";
        }
    }

    string TriggerRandomEvent(int playerIdx)
    {
        bool isPositive = Random.value > 0.5f;
        int impactAmount = Random.Range(20000, 100001);

        if (isPositive)
        {
            GameData.Instance.Budgets[playerIdx] += impactAmount;
            return $"<color=green>EVENT: {positiveEvents[Random.Range(0, positiveEvents.Length)]} (+₹{impactAmount:N0})</color>";
        }
        else
        {
            GameData.Instance.Budgets[playerIdx] = Mathf.Max(0, GameData.Instance.Budgets[playerIdx] - impactAmount);
            return $"<color=red>EVENT: {negativeEvents[Random.Range(0, negativeEvents.Length)]} (-₹{impactAmount:N0})</color>";
        }
    }

    // --- UPDATED: IMPROVED NEXT TURN & SCENE TRANSITION LOGIC ---
    public void OnNextTurn()
    {
        GameData gd = GameData.Instance;
        
        // 1. Move to next player
        gd.CurrentPlayerIndex++;

        // 2. If all players have finished their turn in the current round
        if (gd.CurrentPlayerIndex >= gd.PlayerCount)
        {
            gd.CurrentPlayerIndex = 0;
            gd.CurrentRound++;
            gd.ResetTurnConfirmations();
            Debug.Log($"Round Advanced. Now at Round: {gd.CurrentRound}");
        }

        // 3. Transition Check: If current round exceeds total rounds, go to GameOver
        if (gd.CurrentRound > gd.TotalRounds)
        {
            Debug.Log("Game Finished! Loading Results Scene...");
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            RefreshUI();
        }
    }
}
