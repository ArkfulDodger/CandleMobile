using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private InputAction _jumpAction;
    private InputAction _moveAction;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _jumpAction = _playerInput.actions["Jump"];
        _moveAction = _playerInput.actions["Move"];
    }

    private void OnEnable()
    {
        _jumpAction.started += OnJump;
    }
    private void OnDisable()
    {
        _jumpAction.started -= OnJump;
    }

    private void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>() * _speed;
        Vector3 moveVector = new Vector3(_moveInput.x, _rb.velocity.y, _moveInput.y);
        _rb.velocity = moveVector;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}
