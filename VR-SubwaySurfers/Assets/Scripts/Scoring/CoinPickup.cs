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
            ScoreManager.Instance.AddCoin();
            Destroy(gameObject);
        }
    }
}
