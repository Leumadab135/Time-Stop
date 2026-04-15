using UnityEngine;

public class TimeStopAbilityAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _abilityClip;

    public bool HasValidSetup => _audioSource != null && _abilityClip != null;

    public void PlayAbilitySound()
    {
        if (!HasValidSetup)
        {
            return;
        }

        _audioSource.PlayOneShot(_abilityClip);
    }
}