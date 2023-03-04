using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private float jumpMagnitude = 200f;
    [SerializeField] private float movementMagnitude = 5f;
    [SerializeField] private float airMovementMultiplier = 0.5f; // horizontal force weaker if in air

    // Variable jump height related variables
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float jumpAcceleration = 5f;

    private PlayerInputActions _inputActions;
    private bool _isJumping = false;
    private float _startJumpTime;
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    private void FixedUpdate()
    {
        CheckGroundMovement();
        CheckJump();
    }

    private void CheckGroundMovement()
    {
        var direction = _inputActions.Player.Move.ReadValue<Vector2>();
        
        // We don't want vertical movement to be handled by 'W' and 'S', so we set y to 0.
        if (direction.y < 0) DropDown();
        direction.y = 0;
        
        if (_isJumping)
        {
            direction *= airMovementMultiplier;
        }
        
        playerBody.AddForce(direction * movementMagnitude);
    }

    private void CheckJump()
    {
        bool jump = _inputActions.Player.Jump.ReadValue<float>() > 0.5;
        if (!jump) return;
        
        // First jump
        // Each press will only contribute to one first jump.
        // If the player continues pressing the jump key, when the object hits the ground, the object will not jump continuously.
        // Players have to release the jump key and press it again to perform another jump.
        if (!_isJumping)
        {
            Debug.Log("Jump");
            _isJumping = true;
            _startJumpTime = Time.time;
            playerBody.AddForce(Vector2.up * jumpMagnitude);
        }
        else if (IsBelowMaxJump()) 
        {
            // Variable jump height
            playerBody.AddForce(Vector2.up * jumpAcceleration);
        }
    }

    private bool IsBelowMaxJump()
    {
        return Time.time - _startJumpTime < maxJumpTime;
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
