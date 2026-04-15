using UnityEngine;

[RequireComponent(typeof(CameraCollisionResolver))]
public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private PlayerInputReader _inputReader;

    [Header("Shoulder Camera")]
    [SerializeField] private Vector3 _pivotOffset = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 _shoulderOffset = new Vector3(0.75f, 0.2f, 0f);
    [SerializeField] private float _cameraDistance = 4f;

    [Header("Rotation")]
    [SerializeField] private float _lookSensitivity = 0.15f;
    [SerializeField] private float _minPitch = -30f;
    [SerializeField] private float _maxPitch = 60f;

    [Header("Smoothing")]
    [SerializeField] private float _positionSmoothSpeed = 15f;

    private CameraCollisionResolver _collisionResolver;

    private float _yaw;
    private float _pitch;
    private Vector3 _currentPosition;

    public Quaternion CurrentRotation => Quaternion.Euler(_pitch, _yaw, 0f);
    public Vector3 PivotPosition => _target.position + _pivotOffset;

    private void Awake()
    {
        _collisionResolver = GetComponent<CameraCollisionResolver>();
    }

    private void Start()
    {
        Vector3 initialEuler = transform.eulerAngles;
        _yaw = initialEuler.y;
        _pitch = initialEuler.x;

        _currentPosition = transform.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        HandleRotation();
        UpdateCameraPosition();
    }

    private void HandleRotation()
    {
        Vector2 lookInput = _inputReader.LookInput;

        _yaw += lookInput.x * _lookSensitivity;
        _pitch -= lookInput.y * _lookSensitivity;

        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = CurrentRotation;
        Vector3 pivotPosition = PivotPosition;

        Vector3 desiredShoulderPosition = pivotPosition + rotation * _shoulderOffset;
        Vector3 backwardDirection = rotation * Vector3.back;
        Vector3 desiredCameraPosition = desiredShoulderPosition + backwardDirection * _cameraDistance;

        Vector3 resolvedPosition = _collisionResolver.ResolvePosition(desiredShoulderPosition, desiredCameraPosition);

        _currentPosition = Vector3.Lerp(
            _currentPosition,
            resolvedPosition,
            _positionSmoothSpeed * Time.deltaTime
        );

        transform.position = _currentPosition;
        transform.rotation = rotation;
    }
}