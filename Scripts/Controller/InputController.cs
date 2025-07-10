using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(PlayerInput))]
public partial class InputController : MonoBehaviour
{
    private void OnDisable()
    {
        OnDisableBase();
    }

    private void OnEnable()
    {
        OnEnableBase();
    }

    private void Awake()
    {
        AwakeBase();
    }

    partial void OnDisableBase();
    partial void OnEnableBase();
    partial void AwakeBase();
}

public partial class InputController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Fire { get; private set; }
    public bool Run { get; private set; }
    public bool Jump { get; private set; }
    public bool Crouch { get; private set; }

    private InputActionMap _currentMap;
    public InputAction LookAction { private set; get; }
    public InputAction MoveAction { private set; get; }
    public InputAction FireAction { private set; get; }
    public InputAction RunAction { private set; get; }
    public InputAction JumpAction { private set; get; }
    public InputAction CrouchAction { private set; get; }
    public InputAction InteractAction { private set; get; }
    public InputAction SwitchCursorAction { private set; get; }
    public InputAction PauseAction { private set; get; }

    partial void OnEnableBase()
    {
        EnhancedTouchSupport.Enable();
        _currentMap.Enable();

        LookAction.performed += OnLook;
        MoveAction.performed += OnMove;
        FireAction.performed += OnFire;
        RunAction.performed += OnRun;
        JumpAction.performed += OnJump;
        CrouchAction.started += OnCrouch;

        LookAction.canceled += OnLook;
        MoveAction.canceled += OnMove;
        FireAction.canceled += OnFire;
        RunAction.canceled += OnRun;
        JumpAction.canceled += OnJump;
        CrouchAction.canceled += OnCrouch;

        SwitchCursorAction.performed += SwitchCursor;
    }

    partial void OnDisableBase()
    {
        EnhancedTouchSupport.Disable();
        _currentMap.Disable();

        MoveAction.performed -= OnMove;
        FireAction.performed -= OnFire;
        RunAction.performed -= OnRun;
        JumpAction.performed -= OnJump;
        CrouchAction.started -= OnCrouch;

        MoveAction.canceled -= OnMove;
        FireAction.canceled -= OnFire;
        RunAction.canceled -= OnRun;
        JumpAction.canceled -= OnJump;
        CrouchAction.canceled -= OnCrouch;

        SwitchCursorAction.performed -= SwitchCursor;
    }

    partial void AwakeBase()
    {
        GameManager.Instance.SetCursor(false);
        _currentMap = playerInput.currentActionMap;
        LookAction = _currentMap.FindAction("Look");
        MoveAction = _currentMap.FindAction("Move");
        FireAction = _currentMap.FindAction("Fire");
        RunAction = _currentMap.FindAction("Run");
        JumpAction = _currentMap.FindAction("Jump");
        CrouchAction = _currentMap.FindAction("Crouch");
        InteractAction = _currentMap.FindAction("Interact");
        SwitchCursorAction = _currentMap.FindAction("Switch Cursor");
        PauseAction = _currentMap.FindAction("Pause");
    }

    private void SwitchCursor(InputAction.CallbackContext context)
    {
        GameManager.Instance.SetCursor(!Cursor.visible);
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Fire = context.ReadValueAsButton();
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        Run = context.ReadValueAsButton();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        Jump = context.ReadValueAsButton();
    }
    private void OnCrouch(InputAction.CallbackContext context)
    {
        Crouch = context.ReadValueAsButton();
    }
}