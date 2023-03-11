using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private float movementMagnitude = 5f;

    [Header("Jump related variables")]
    [SerializeField] private float jumpMagnitude = 200f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float jumpAcceleration = 5f;
    [SerializeField] private float airMovementMultiplier = 0.5f; // horizontal force weaker if in air
    [SerializeField] private float groundDetectionSensitivity = 0.52f;

    [SerializeField] private GameHandler _gameHandler;

    private PlayerInputActions _inputActions;
    public bool _isOnGround = true;
    private float _startJumpTime;

    private Vector2 _size;
    private Vector2 _sideOffset;
    private Vector2 _heightOffset;
    private float _rayDist;
    private List<RaycastHit2D> _grounds;
    
    private LayerMask _groundMask;
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        
        _groundMask = LayerMask.GetMask("Ground");
        
        Vector2 size = transform.localScale;
        _sideOffset = new Vector2(size.x * 0.5f, 0);
        _rayDist = size.y * groundDetectionSensitivity;
    }

    private void FixedUpdate()
    {
        if (_gameHandler.isRunning())
        {
            _grounds = CheckOnGround();
            _isOnGround = _grounds.Any(e => e); // Checks if at least 1 elem is not null
            GetHorizontalInput();
            GetJumpInput();
        }
    }

    private void GetHorizontalInput()
    {
        var direction = _inputActions.Player.Move.ReadValue<Vector2>();
        
        // We don't want vertical movement to be handled by 'W' and 'S', so we set y to 0.
        if (direction.y < 0) DropDown();
        direction.y = 0;
        
        if (!_isOnGround)
        {
            direction *= airMovementMultiplier;
        }
        
        playerBody.AddForce(direction * movementMagnitude);
    }

    private void GetJumpInput()
    {
        bool jump = _inputActions.Player.Jump.ReadValue<float>() > 0.5;
        if (!jump) return;
        
        // First jump
        // Each press will only contribute to one first jump.
        // If the player continues pressing the jump key, when the object hits the ground, the object will not jump continuously.
        // Players have to release the jump key and press it again to perform another jump.
        if (_isOnGround)
        {
            Debug.Log("Jump");
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
        if (_grounds.Where(g => g).Any(g => g.transform.CompareTag("DropDownable")))
        {
            Debug.Log("Drop Down");
        }
    }

    private List<RaycastHit2D> CheckOnGround()
    {
        Vector2 center = transform.position;

        return new List<RaycastHit2D>()
        {
            Physics2D.Raycast(center + _sideOffset, Vector2.down, _rayDist, _groundMask),
            Physics2D.Raycast(center - _sideOffset, Vector2.down, _rayDist, _groundMask)
        };
}
}
