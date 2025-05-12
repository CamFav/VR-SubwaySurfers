using UnityEngine;

/// <summary>
/// Handles the collection of coins by the player. 
/// </summary>
public class CoinPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.coinSfx);
            ScoreManager.Instance.AddCoin();
            Destroy(gameObject);
        }
    }
}
