using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData _stats;
    
    #region Private

    private Rigidbody2D _rb;
    
    private FrameInput _input;
    private PlayerInput _playerInput;

    private Vector2 _speed;
    private int _fixedFrame;
    private bool _grounded;

    #endregion

    #region Public

    public PlayerData Stats => _stats;
    
    public Vector2 Speed => _speed;

    #endregion
    
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        _input = _playerInput.FrameInput;
    }

    private void FixedUpdate()
    {
        _fixedFrame++;
        
        HandleHorizontal();
        HandleJump();
        
        ApplyVelocity();
    }

    #region Collision

    private int _frameLeftGrounded = int.MinValue;

    #endregion

    #region Horizontal

    private void HandleHorizontal()
    {
        if (_input.Move.x != 0)
        {
            // Prevent useless holrizontal speed buildup when going agains a wall
            if (Mathf.Approximately(_rb.velocity.x, 0) && Math.Abs(Mathf.Sign(_input.Move.x) - Mathf.Sign(_speed.x)) < 0.01f)
                _speed.x = 0;

            float inputX = _input.Move.x;
            _speed.x = Mathf.MoveTowards(_speed.x, inputX * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            Debug.Log(_speed.x);
        }
    }

    #endregion

    #region Jump

    private bool _jumpToConsume;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private bool _bufferedJumpUsable;
    private int _frameJumpWasPressed = int.MinValue;

    private bool CanUseCoyote => 
        _coyoteUsable && !_grounded && _fixedFrame < _frameLeftGrounded + _stats.CoyoteFrames;
    private bool HasBufferedJump =>
        _grounded && _bufferedJumpUsable && _fixedFrame < _frameJumpWasPressed + _stats.JumpBufferFrames;
    
    private void HandleJump()
    {
        // standard jump
        if ((_jumpToConsume && CanUseCoyote) || HasBufferedJump)
        {
            _coyoteUsable = false;
        }

        // early jump end detection
    }

    private void ResetJump()
    {
        _coyoteUsable = true;
        _bufferedJumpUsable = true;
        _endedJumpEarly = false;
    }

    #endregion
    
    private void ApplyVelocity()
    {
        _rb.velocity = _speed;
    }
}
