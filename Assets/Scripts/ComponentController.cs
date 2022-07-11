using UnityEngine;
using UnityEngine.InputSystem;

public class ComponentController : MonoBehaviour
{
    // reference variables
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private Animator _animator;

    // input variables
    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _jumpAction;

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
        _playerInput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // assigning input actions
        _moveAction = _playerInput.actions["Move"];
        _runAction = _playerInput.actions["Run"];
        _jumpAction = _playerInput.actions["Jump"];

        // assigning animation hashes
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
    }

    private void OnEnable()
    {
        _moveAction.started += OnMovementInput;
        _moveAction.performed += OnMovementInput;
        _moveAction.canceled += OnMovementInput;
        _runAction.started += OnRunInput;
        _runAction.canceled += OnRunInput;
    }

    private void OnDisable()
    {
        _moveAction.started -= OnMovementInput;
        _moveAction.performed -= OnMovementInput;
        _moveAction.canceled -= OnMovementInput;
        _runAction.started -= OnRunInput;
        _runAction.canceled -= OnRunInput;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGravity();
        UpdateMovement();
        UpdateRotation();
        UpdateAnimation();
    }

    // updates on all Movement input events
    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.magnitude != 0;
    }

    // updates on run trigger and release
    void OnRunInput(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    // update gravity for this frame
    void UpdateGravity()
    {
        if (_characterController.isGrounded)
        {
            _currentMovement.y = _groundedGravity;
        }
        else
        {
            _currentMovement.y += _gravity * Time.deltaTime;
        }
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
        {
            _animator.SetBool(_isWalkingHash, true);
        }
        else if (!_isMovementPressed && isWalking)
        {
            _animator.SetBool(_isWalkingHash, false);
        }

        if (_isMovementPressed && _isRunPressed && !isRunning)
        {
            _animator.SetBool(_isRunningHash, true);
        }
        else if ((!_isMovementPressed || !_isRunPressed) && isRunning)
        {
            _animator.SetBool(_isRunningHash, false);
        }
    }
}
