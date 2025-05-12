using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// <summary>
// Handles the game over state and UI.
// </summary>
public class GameOverManager : MonoBehaviour
{
[SerializeField] private CanvasGroup gameOverCanvas;
[SerializeField] private GameObject gameOverCanvasRoot;
[SerializeField] private float fadeDuration = 1f;
[SerializeField] private float waitBeforeReload = 2f;
private ScoreManager scoreManager;

void Awake()
{
    scoreManager = ScoreManager.Instance;
}
public void TriggerGameOver()
{
    StartCoroutine(FadeThenReloadScene());
}

private IEnumerator FadeThenReloadScene()
{
    gameOverCanvasRoot.SetActive(true);

    float t = 0f;

    // Fade to black
    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        gameOverCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
        yield return null;
    }

    int score = ScoreManager.Instance.GetScore();
    int coins = ScoreManager.Instance.GetCoins();

    LeaderboardManager.AddEntry(score, coins);


    yield return new WaitForSeconds(waitBeforeReload);

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
}