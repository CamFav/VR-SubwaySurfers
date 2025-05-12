using UnityEngine;

/// <summary>
/// Randomly selects a obstacle
public class RandomObstacleSelector : MonoBehaviour
{
    void Start()
    {
        int childCount = transform.childCount;
        int index = Random.Range(0, childCount);

        for (int i = 0; i < childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == index);
    }
}
