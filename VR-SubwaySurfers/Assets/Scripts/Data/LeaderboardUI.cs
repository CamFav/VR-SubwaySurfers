using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform entryContainer;
    public GameObject leaderboardPanel;

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);

        var data = LeaderboardManager.Load();

        // Destroy existing entries
        foreach (Transform child in entryContainer)
            Destroy(child.gameObject);

        // Create new entries
        foreach (var entry in data.scores)
        {
            GameObject go = Instantiate(entryPrefab, entryContainer);
            LeaderboardEntryUI ui = go.GetComponent<LeaderboardEntryUI>();
            ui.Setup(entry.score, entry.coins);
        }
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }
}
