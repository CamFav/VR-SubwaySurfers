using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LeaderboardManager
{
    private static string path => Application.persistentDataPath + "/leaderboard.json";
    private const int maxEntries = 5;

    public static LeaderboardData Load()
    {
        if (!File.Exists(path))
            return new LeaderboardData();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<LeaderboardData>(json);
    }

    public static void Save(LeaderboardData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static void AddEntry(int score, int coins)
    {
        var data = Load();
        data.scores.Add(new ScoreEntry { score = score, coins = coins });
        data.scores.Sort((a, b) => b.score.CompareTo(a.score));

        if (data.scores.Count > maxEntries)
            data.scores.RemoveRange(maxEntries, data.scores.Count - maxEntries);

        Save(data);
    }
}
