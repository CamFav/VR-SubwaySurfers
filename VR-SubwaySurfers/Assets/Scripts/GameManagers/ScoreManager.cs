using TMPro;
using UnityEngine;

/// <summary>
/// Manages displaying of the score and coins collected.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;

    [Header("Scoring")]
    public Transform worldRootTransform;
    private float startZ;
    private bool isCounting = false;
    private int coins = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartScoring()
    {
        if (worldRootTransform != null)
            startZ = worldRootTransform.position.z;

        isCounting = true;
    }

    void Update()
    {
        if (!isCounting || worldRootTransform == null) return;

        float distance = Mathf.Abs(worldRootTransform.position.z - startZ);
        UpdateScoreDisplay(distance);
    }

    public void AddCoin()
    {
        coins++;
        UpdateCoinsDisplay();
    }

    private void UpdateScoreDisplay(float distance)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(distance);
    }

    private void UpdateCoinsDisplay()
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + coins;
    }
}
