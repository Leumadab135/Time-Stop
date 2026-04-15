using UnityEngine;

public class CameraCollisionResolver : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private float _cameraRadius = 0.2f;
    [SerializeField] private float _collisionOffset = 0.1f;

    public Vector3 ResolvePosition(Vector3 shoulderPosition, Vector3 desiredCameraPosition)
    {
        Vector3 direction = desiredCameraPosition - shoulderPosition;
        float distance = direction.magnitude;

        if (distance <= 0f)
        {
            return desiredCameraPosition;
        }

        direction.Normalize();

        if (Physics.SphereCast(
                shoulderPosition,
                _cameraRadius,
                direction,
                out RaycastHit hit,
                distance,
                _collisionMask,
                QueryTriggerInteraction.Ignore))
        {
            return hit.point - direction * _collisionOffset;
        }

        return desiredCameraPosition;
    }
}