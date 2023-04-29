using UnityEngine;

/// <summary>
/// This class acts as an easy-to-get component that can be found from the hole's LevelEnd trigger
/// so that the celebration particles can be triggered when the ball goes into the hole.
/// </summary>
public class GolfHoleData : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem celebrationParticles;

    public void PlayCelebration()
    {
        celebrationParticles.Play();
    }
}