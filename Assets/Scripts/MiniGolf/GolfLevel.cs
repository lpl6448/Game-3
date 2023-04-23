using UnityEngine;

public class GolfLevel : MonoBehaviour
{
    // Name of this level, taken from the GameObject's name
    public string Name => gameObject.name;

    // Name displayed on-screen at the beginning of this level
    public string DisplayName;

    // Max number of strokes that can be taken to beat this level
    public int Par = 3;

    // Transform representing the location where the golf ball will spawn
    public Transform BallSpawn;

    // Transform representing the position/rotation of the initial zoomed-out camera angle for this level
    public Transform IntroCameraPosition;

    // Transform representing the location that the camera focuses on when it zooms into the ball
    public Transform LevelIntroFocus;

    // Whether this level has bounding boxes that can define a more precise "in-bounds" area
    // If it is false, the ball respawns if it goes below y = -10
    public bool HasBounds = false;
}