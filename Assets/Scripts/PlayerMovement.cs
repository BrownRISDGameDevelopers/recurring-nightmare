using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : GroundDetectionEntity
{
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private float movementMagnitude = 5f;
    [SerializeField] private float maxHorizontalSpeed = 8f;

    [Header("Jump related variables")] 
    [SerializeField] private float jumpMagnitude = 200f;

    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float jumpAcceleration = 5f;
    [SerializeField] private float airMovementMultiplier = 0.5f; // horizontal force weaker if in air
    
    private PlayerInputActions _inputActions;
    private bool _isOnGround;
    private bool _pressedJumpPrevFrame = false;
    private float _startJumpTime;

    [Header("Audio")]
    [SerializeField] private AudioSource runningAudioSource;
    [SerializeField] private AudioSource jumpInitiateAudioSource;
    [SerializeField] private AudioSource jumpImpactAudioSource;

    protected override void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        
        base.Awake();
    }

    private void FixedUpdate()
    {
        if (GameManager.GameState != GameManager.RunningState.Running) return;
        bool previousOnGround = _isOnGround;
        (_isOnGround, _) = CheckOnGround();
        if (!previousOnGround && _isOnGround) jumpImpactAudioSource.Play();
        GetHorizontalInput();
        GetJumpInput();
        CapHorizontalSpeed();
    }

    private void CapHorizontalSpeed()
    {
        Vector3 v = playerBody.velocity;
        v.x = Mathf.Clamp(v.x, -maxHorizontalSpeed, maxHorizontalSpeed);
        playerBody.velocity = v;
    }

    private void GetHorizontalInput()
    {
        var direction = _inputActions.Player.Move.ReadValue<Vector2>();
        
        // We don't want vertical movement to be handled by 'W' and 'S', so we set y to 0.
        direction.y = 0;

        if (direction.x != 0 && !runningAudioSource.isPlaying) runningAudioSource.Play();
        else if (direction.x == 0) runningAudioSource.Stop();

        if (!_isOnGround)
        {
            direction *= airMovementMultiplier;
            runningAudioSource.Stop();
        }
        
        playerBody.AddForce(direction * movementMagnitude);
    }

    private void GetJumpInput()
    {
        bool jump = _inputActions.Player.Jump.ReadValue<float>() > 0.5;
        if (jump)
        {
            // First jump
            // Each press will only contribute to one first jump.
            // If the player continues pressing the jump key, when the object hits the ground, the object will not jump continuously.
            // Players have to release the jump key and press it again to perform another jump.
            if (_isOnGround && !_pressedJumpPrevFrame)
            {
                _startJumpTime = Time.time;
                playerBody.AddForce(Vector2.up * jumpMagnitude);
                jumpInitiateAudioSource.Play();
            }
            else if (IsBelowMaxJump()) 
            {
                // Variable jump height
                playerBody.AddForce(Vector2.up * jumpAcceleration);
            }
        }
        _pressedJumpPrevFrame = jump;
    }

    private bool IsBelowMaxJump()
    {
        return Time.time - _startJumpTime < maxJumpTime;
    }
}
