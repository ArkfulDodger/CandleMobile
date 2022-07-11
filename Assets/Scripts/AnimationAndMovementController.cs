using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    // reference variables
    private PlayerControls _playerControls;
    private CharacterController _characterController;
    private Animator _animator;

    // input variables
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isMovementPressed;
    private float _rotationFactorPerFrame = 13f;

    private void Awake()
    {
        // assigning reference variables
        _playerControls = new PlayerControls();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // enabling player controls input
        _playerControls.CharacterControls.Enable();

        // subscribing to input events
        _playerControls.CharacterControls.Move.started += OnMovementInput;
        _playerControls.CharacterControls.Move.performed += OnMovementInput;
        _playerControls.CharacterControls.Move.canceled += OnMovementInput;
    }

    private void OnDisable()
    {
        // disabling player controls input
        _playerControls.CharacterControls.Disable();

        // unsubscribing from input events
        _playerControls.CharacterControls.Move.started -= OnMovementInput;
        _playerControls.CharacterControls.Move.performed -= OnMovementInput;
        _playerControls.CharacterControls.Move.canceled -= OnMovementInput;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateRotation();
        UpdateAnimation();
    }

    // updates on all Movement input events
    void OnMovementInput (InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.magnitude != 0;
    }

    // execute movement for this frame
    void UpdateMovement()
    {
        _characterController.Move(_currentMovement * Time.deltaTime);
    }

    // execute rotation for this frame
    private void UpdateRotation()
    {
        if (!_isMovementPressed) return;

        Debug.Log("Rotation being Updated");

        Vector3 movementDirection = new(_currentMovement.x, 0.0f, _currentMovement.z);
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        Quaternion currentRotation = transform.rotation;

        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
    }

    // execute animation for this frame
    void UpdateAnimation()
    {
        bool isWalking = _animator.GetBool("isWalking");
        bool isRunning = _animator.GetBool("isRunning");

        if (_isMovementPressed && !isWalking)
        {
            _animator.SetBool("isWalking", true);
        }
        else if (!_isMovementPressed && isWalking)
        {
            _animator.SetBool("isWalking", false);
        }
    }
}
