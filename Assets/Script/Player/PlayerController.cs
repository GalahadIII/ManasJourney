using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData _stats;
    
    #region Private

    private Rigidbody2D _rb;
    [SerializeField] private CapsuleCollider2D _col;

    private FrameInput _input;
    private PlayerInput _playerInput;

    private Vector2 _speed;
    private Vector2 _currentExternalVelocity;
    private int _fixedFrame;
    private bool _hasControl = true;
    private bool _cachedTriggerSetting;

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

    private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[2];
    private readonly RaycastHit2D[] _ceilingHits = new RaycastHit2D[2];
    private readonly Collider2D[] _wallHits = new Collider2D[5];
    private Vector2 _groundNormal;
    private int _groundHitCount;
    private int _ceilingHitCount;
    private int _wallHitCount;
    private int _frameLeftGrounded = int.MinValue;
    private bool _grounded;

    private void CheckCollisions()
    {
        // TODO work on my understanding of this mess
        _groundHitCount = Physics2D.CapsuleCastNonAlloc(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down,
            _groundHits, _stats.GrounderDistance, ~_stats.PlayerLayer);
        _ceilingHitCount = Physics2D.CapsuleCastNonAlloc(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, 
            _ceilingHits, _stats.GrounderDistance, ~_stats.PlayerLayer);
    }
    
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
            _bufferedJumpUsable = false;
            _speed.y = _stats.JumpPower;
        }

        // early jump end detection
        if (!_endedJumpEarly && !_grounded && !_input.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;
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
        if (!_hasControl) return;
        _rb.velocity = _speed + _currentExternalVelocity;

        _currentExternalVelocity = Vector2.MoveTowards(_currentExternalVelocity, Vector2.zero, _stats.ExternalVelocityDecay * Time.fixedDeltaTime);
    }
}
