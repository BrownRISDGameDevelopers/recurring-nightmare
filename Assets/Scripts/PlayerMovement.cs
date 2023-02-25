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
    private BoxCollider2D _boxColliderPlayer;
    private Transform _transform;
    private float _player_height;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _transform = GetComponent<Transform>();
        _boxColliderPlayer = GetComponent<BoxCollider2D>();
        _player_height = _boxColliderPlayer.bounds.extents.y;
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
        Debug.Log(isGrounded());
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

    bool isGrounded()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(_boxColliderPlayer.bounds.center, Vector2.down, _player_height);
        bool isGrounded = hit.collider != null;

        Debug.Log(_boxColliderPlayer.bounds.center);
        Debug.Log(_player_height);

        Debug.DrawRay(_boxColliderPlayer.bounds.center, Vector2.down * _player_height, isGrounded ? Color.green : Color.red, 0.5f);
        return isGrounded;
    }
}
