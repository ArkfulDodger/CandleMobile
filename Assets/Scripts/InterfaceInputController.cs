using UnityEngine;
using UnityEngine.InputSystem;

public class InterfaceInputController : MonoBehaviour, PlayerControls.ICharacterControlsActions
{
    // reference variables
    private PlayerControls _playerControls;
    private CharacterController _characterController;
    private Animator _animator;

    // animation variables
    private int _isWalkingHash;
    private int _isRunningHash;

    // input variables
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isMovementPressed;
    private bool _isRunPressed;

    // adjustable variables
    private readonly float _runMultiplier = 3.0f;
    private readonly float _rotationFactorPerFrame = 15f;
    private readonly float _groundedGravity = -0.05f;
    private readonly float _gravity = -9.8f;

    private void Awake()
    {
        // assigning reference variables
        _playerControls = new PlayerControls();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _playerControls.CharacterControls.SetCallbacks(this);

        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
    }

    private void OnEnable()
    {
        // enabling player controls input
        _playerControls.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        // disabling player controls input
        _playerControls.CharacterControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGravity();
        UpdateMovement();
        UpdateRotation();
        UpdateAnimation();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.magnitude != 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Pressed");
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    // update gravity for this frame
    void UpdateGravity()
    {
        if (_characterController.isGrounded)
            _currentMovement.y = _groundedGravity;
        else
            _currentMovement.y += _gravity * Time.deltaTime;
    }

    // execute movement for this frame
    void UpdateMovement()
    {
        // update speed for running or walking
        float runMultiplier = _isRunPressed ? _runMultiplier : 1f;
        _currentMovement.x = _currentMovementInput.x * runMultiplier;
        _currentMovement.z = _currentMovementInput.y * runMultiplier;

        // apply movement to character controller
        _characterController.Move(_currentMovement * Time.deltaTime);
    }

    // execute rotation for this frame
    private void UpdateRotation()
    {
        if (!_isMovementPressed) return;

        Vector3 movementDirection = new(_currentMovement.x, 0.0f, _currentMovement.z);
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        Quaternion currentRotation = transform.rotation;

        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
    }

    // execute animation for this frame
    void UpdateAnimation()
    {
        bool isWalking = _animator.GetBool(_isWalkingHash);
        bool isRunning = _animator.GetBool(_isRunningHash);

        if (_isMovementPressed && !isWalking)
            _animator.SetBool(_isWalkingHash, true);
        else if (!_isMovementPressed && isWalking)
            _animator.SetBool(_isWalkingHash, false);

        if (_isMovementPressed && _isRunPressed && !isRunning)
            _animator.SetBool(_isRunningHash, true);
        else if ((!_isMovementPressed || !_isRunPressed) && isRunning)
            _animator.SetBool(_isRunningHash, false);
    }
}
