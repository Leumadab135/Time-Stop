using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerMovementMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _playerVisualRoot;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _rotationSpeed = 12f;
    [SerializeField] private float _gravity = -25f;

    private CharacterController _characterController;
    private PlayerInputReader _inputReader;

    private Vector3 _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputReader = GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleGravity();
    }

    private void HandleRotation()
    {
        Vector3 cameraForward = _cameraTransform.forward;
        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude < 0.001f)
        {
            return;
        }

        cameraForward.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        _playerVisualRoot.rotation = Quaternion.Slerp(
            _playerVisualRoot.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _inputReader.MoveInput;

        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        Vector3 horizontalVelocity = moveDirection * _moveSpeed;
        Vector3 finalVelocity = horizontalVelocity + _verticalVelocity;

        _characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity.y < 0f)
        {
            _verticalVelocity.y = -2f;
        }
        else
        {
            _verticalVelocity.y += _gravity * Time.deltaTime;
        }
    }
}