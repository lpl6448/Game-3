using UnityEngine;

public class GolfLevel : MonoBehaviour
{
    // Name of this level, taken from the GameObject's name
    public string Name => gameObject.name;

    // Max number of strokes that can be taken to beat this level
    public int Par = 3;

    // Transform representing the location where the golf ball will spawn
    public Transform BallSpawn;
}