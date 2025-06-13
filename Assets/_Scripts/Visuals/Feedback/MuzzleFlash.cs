using UnityEngine;

public class MuzzleFlash1 : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem muzzleParticleSystem;

    public void PlayMuzzleFlash()
        => muzzleParticleSystem.Play();
    
    
}
