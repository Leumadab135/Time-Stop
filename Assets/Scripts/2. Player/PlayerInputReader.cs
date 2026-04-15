using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsFireHeld { get; private set; }
    public bool TimeStopPressedThisFrame { get; private set; }

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Gameplay.Move.performed += OnMovePerformed;
        _inputActions.Gameplay.Move.canceled += OnMoveCanceled;

        _inputActions.Gameplay.Look.performed += OnLookPerformed;
        _inputActions.Gameplay.Look.canceled += OnLookCanceled;

        _inputActions.Gameplay.Fire.performed += OnFirePerformed;
        _inputActions.Gameplay.Fire.canceled += OnFireCanceled;

        _inputActions.Gameplay.TimeStop.performed += OnTimeStopPerformed;
    }

    private void OnDisable()
    {
        _inputActions.Gameplay.Move.performed -= OnMovePerformed;
        _inputActions.Gameplay.Move.canceled -= OnMoveCanceled;

        _inputActions.Gameplay.Look.performed -= OnLookPerformed;
        _inputActions.Gameplay.Look.canceled -= OnLookCanceled;

        _inputActions.Gameplay.Fire.performed -= OnFirePerformed;
        _inputActions.Gameplay.Fire.canceled -= OnFireCanceled;

        _inputActions.Gameplay.TimeStop.performed -= OnTimeStopPerformed;

        _inputActions.Disable();
    }

    private void LateUpdate()
    {
        TimeStopPressedThisFrame = false;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        LookInput = Vector2.zero;
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        IsFireHeld = true;
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        IsFireHeld = false;
    }

    private void OnTimeStopPerformed(InputAction.CallbackContext context)
    {
        TimeStopPressedThisFrame = true;
    }
}