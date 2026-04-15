using UnityEngine;

[RequireComponent(typeof(EnemyDetection))]
[RequireComponent(typeof(Health))]
public class EnemyAudioController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip _detectionClip;
    [SerializeField] private AudioClip _deathClip;

    private EnemyDetection _detection;
    private Health _health;

    private void Awake()
    {
        _detection = GetComponent<EnemyDetection>();
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _detection.OnPlayerDetected += PlayDetectionSound;
        _health.OnDied += PlayDeathSound;
    }

    private void OnDisable()
    {
        _detection.OnPlayerDetected -= PlayDetectionSound;
        _health.OnDied -= PlayDeathSound;
    }

    private void PlayDetectionSound()
    {
        if (_audioSource == null || _detectionClip == null)
        {
            return;
        }

        _audioSource.PlayOneShot(_detectionClip);
    }

    private void PlayDeathSound()
    {
        if (_deathClip != null)
        {
            AudioSource.PlayClipAtPoint(_deathClip, transform.position);
        }
    }
}