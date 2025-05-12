using UnityEngine;

public class LeaderboardDebugReset : MonoBehaviour
{
    [ContextMenu("Réinitialiser le leaderboard")]
    public void ResetLeaderboard()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "leaderboard.json");

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            Debug.Log("Leaderboard réinitialisé");
        }
        else
        {
            Debug.Log("Aucun fichier leaderboard.json trouvé");
        }
    }
}
