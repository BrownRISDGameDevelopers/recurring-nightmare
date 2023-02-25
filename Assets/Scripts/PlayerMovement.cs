using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private float jumpMagnitude = 100f;
    [SerializeField] private float movementMagnitude = 5f;

    private Rigidbody2D _rigidbody;
    private PlayerInput _playerInput;
    private PlayerInputActions _inputActions;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        _inputActions.Player.Jump.performed += Jump;
    }

    private void FixedUpdate()
    {
        CheckMovement();
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

    private void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        Debug.Log("Jump");
        playerBody.AddForce(Vector2.up * jumpMagnitude);
    }


    // Drops down to platform below if plat form is 'dropdownable'
    private void DropDown()
    {
        Debug.Log("Drop Down");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
    }
}
