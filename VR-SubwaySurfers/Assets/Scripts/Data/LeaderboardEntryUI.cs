using TMPro;
using UnityEngine;

public class LeaderboardEntryUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;

    public void Setup(int score, int coins)
    {
        scoreText.text = $"Score: {score}";
        coinsText.text = $"Coins: {coins}";
    }
}