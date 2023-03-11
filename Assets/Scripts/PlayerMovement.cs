using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : GroundDetectionEntity
{
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private float movementMagnitude = 5f;

    [Header("Jump related variables")]
    [SerializeField] private float jumpMagnitude = 200f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float jumpAcceleration = 5f;
    [SerializeField] private float airMovementMultiplier = 0.5f; // horizontal force weaker if in air

    [SerializeField] private GameHandler _gameHandler;

    private PlayerInputActions _inputActions;
    private bool _isOnGround;
    private float _startJumpTime;

    private List<RaycastHit2D> _groundSurfaces;
    
    protected override void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        
        base.Awake();
    }

    private void FixedUpdate()
    {
        if (_gameHandler.isRunning())
        {
            (_isOnGround, _groundSurfaces) = CheckOnGround();
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

    private bool CheckDropDownable()
    {
        return _groundSurfaces.Where(g => g).All(g => g.transform.CompareTag("DropDownable"));
    }
    
    // Drops down to platform below if plat form is 'dropdownable'
    private void DropDown()
    {
        var surfaces = _groundSurfaces.Where(g => g);
        if (!surfaces.All(g => g.transform.CompareTag("DropDownable"))) return;
        
        
    }
}
