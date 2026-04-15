using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _aimOrigin;

    [Header("Aim Settings")]
    [SerializeField] private float _maxAimDistance = 200f;
    [SerializeField] private LayerMask _aimMask;

    public Vector3 CurrentAimPoint { get; private set; }
    public Vector3 CurrentAimDirection { get; private set; }

    private void Update()
    {
        UpdateAim();
    }

    private void UpdateAim()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, _maxAimDistance, _aimMask, QueryTriggerInteraction.Ignore))
        {
            CurrentAimPoint = hit.point;
        }
        else
        {
            CurrentAimPoint = ray.origin + ray.direction * _maxAimDistance;
        }

        CurrentAimDirection = (CurrentAimPoint - _aimOrigin.position).normalized;
    }
}