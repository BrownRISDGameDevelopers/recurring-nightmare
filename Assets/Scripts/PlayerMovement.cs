using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private float jumpMagnitude = 200f;
    [SerializeField] private float movementMagnitude = 5f;

    // Variable jump related variables
    [SerializeField] private float _maxJumpTime = 0.5f;
    [SerializeField] private float _jumpAcceleration = 5f;

    private Rigidbody2D _rigidbody;
    private PlayerInput _playerInput;
    private PlayerInputActions _inputActions;
    // Variable jump related variables
    private bool _isJumping = false;
    private bool _pressingJump = false;
    private float _startJumpTime;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    private void FixedUpdate()
    {
        CheckMovement();
        CheckJump();
    }

    private void CheckMovement()
    {
        var direction = _inputActions.Player.Move.ReadValue<Vector2>();
        if (direction.y < 0)
        {
            DropDown();
        }
        
        // We don't want vertical movement to be handled by 'W' and 'S'
        direction.y = 0;
        playerBody.AddForce(direction * movementMagnitude);
    }

    private void CheckJump()
    {
        // If the key is pressing
        if (_inputActions.Player.Jump.ReadValue<float>() > 0.5)
        {
            // First jump
            // Each press will only contribute to one first jump.
            // If the player continues pressing the jump key, when the object hits the ground, the object will not jump continuously.
            // Players have to release the jump key and press it again to perform another jump.
            if (!_pressingJump && !_isJumping)
            {
                Debug.Log("Jump");
                _isJumping = true;
                _startJumpTime = Time.time;
                playerBody.AddForce(Vector2.up * jumpMagnitude);
            }
            // Variable jump height
            if (_pressingJump && _isJumping && Time.time - _startJumpTime < _maxJumpTime)
            {
                playerBody.AddForce(Vector2.up * _jumpAcceleration);
            }
            _pressingJump = true;
        }
        else
        {
            _pressingJump = false;
        }
    }


    // Drops down to platform below if plat form is 'dropdownable'
    private void DropDown()
    {
        Debug.Log("Drop Down");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        _isJumping = false;
    }
}
