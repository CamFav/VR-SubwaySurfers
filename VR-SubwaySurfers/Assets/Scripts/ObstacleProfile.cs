using UnityEngine;

[CreateAssetMenu(menuName = "Obstacle/ObstacleProfile")]
public class ObstacleProfile : ScriptableObject
{
    public GameObject prefab;
    public float length = 3f;
    public enum Category { Solid, Jumpable, Slideable }
    public Category category;
}
