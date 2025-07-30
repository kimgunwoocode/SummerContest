using UnityEngine;

public class PaticleInteractor : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void StartParticle()
    {
        _particleSystem.Play();
    }
}
