using UnityEngine;

public class GolfLevel : MonoBehaviour
{
    // Name of this level, taken from the GameObject's name
    public string Name => gameObject.name;

    // Max number of strokes that can be taken to beat this level
    public int Par = 3;

    // Transform representing the location where the golf ball will spawn
    public Transform BallSpawn;

    // Whether this level has bounding boxes that can define a more precise "in-bounds" area
    // If it is false, the ball respawns if it goes below y = -10
    public bool HasBounds = false;
}