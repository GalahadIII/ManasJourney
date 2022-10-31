using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private PlayerMoveStats _stats;

        #region Private

        private Rigidbody2D _rb;
        private PlayerInput _input;
        private FrameInput _frameInput;
        
        private int _fixedUpdateCounter;
        private bool _dashing;
        private bool _hasControl;
        private bool _grounded;
        private bool _facingRight;
        private float _gravityScale;

        #endregion

        #region Public

        public Vector2 Input => _frameInput.Move;
        public Vector2 Speed => _rb.velocity;
        public bool Falling => _rb.velocity.y < 0;
        public bool Jumping => _rb.velocity.y > 0;
        public bool FacingRight => _facingRight;

        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            _gravityScale = _rb.gravityScale;
        }

        private void Update()
        {
            _frameInput = _input.FrameInput;
            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _frameJumpWasPressed = _fixedUpdateCounter;
            }

            if (_frameInput.JumpUp) JumpCut();
        }
        
        private void FixedUpdate()
        {
            _fixedUpdateCounter++;

            // input dependant
            Flip();
            Horizontal();
            Jump();
            ArtificialFriction();
            
            // !input dependant
            FallingGravity();
        }

        #region Horizontal

        private void Horizontal()
        {
            if (_dashing) return;

            // calculate wanted direction and desired velocity
            float targetSpeed = (_frameInput.Move.x == 0 ? 0 : MathF.Sign(_frameInput.Move.x))  * _stats.MoveSpeed;
            // calculate difference between current volocity and target velocity
            float speedDif = targetSpeed - _rb.velocity.x;
            // change acceleration rate depending on situations;
            float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? _stats.Acceleration : _stats.Decceleration;
            // applies acceleration to speed difference, raise to a set power so acceleration increase with higher speed
            // multiply by sign to reapply direction
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.VelPower) * Mathf.Sign(speedDif);

            // apply the movement force
            _rb.AddForce(movement * Vector2.right);
        }

        private void ArtificialFriction()
        {
            if (!_grounded) return;
            if (!(Mathf.Abs(_frameInput.Move.x) < 0.01f)) return;
            
            // use either friction amount or velocity
            float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_stats.Friction));
            // sets to movement direction
            amount *= Mathf.Sign(_rb.velocity.x);
            // applies force against movement direction
            _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);

        }

        #endregion

        #region Jump

        private bool _jumpToConsume;
        private int _frameJumpWasPressed;
        
        private bool HasBufferedJump => false; // if frameCount < framejumpwaspressed + bufferCout
        private bool CanUseCoyote => false;

        private void Jump()
        {
            _grounded = true;
            if (_jumpToConsume || HasBufferedJump)
            {
                if (_grounded || CanUseCoyote) NormalJump();
            }

            _jumpToConsume = false;
        }

        private void NormalJump()
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(Vector2.up * _stats.JumpForce, ForceMode2D.Impulse);
        }

        private void JumpCut()
        {
            if (!Falling)
            {
                // reduces current y velocity by amount[0-1] (higher the CutMultiplier the less sensitive to input it becomes)
                _rb.AddForce(Vector2.down * (_rb.velocity.y * (1 - _stats.JumpCutMultiplier)), ForceMode2D.Impulse);
            }
        }

        private void FallingGravity()
        {
            _rb.gravityScale = Falling ? _stats.FallGravityMultiplier : _gravityScale;
        }

        #endregion

        private void Flip()
        {
            if (Mathf.Abs(_frameInput.Move.x) < 0.1f) return;
            if (_facingRight && Mathf.Sign(_frameInput.Move.x) < 0) return;
            if (!_facingRight && Mathf.Sign(_frameInput.Move.x) > 0) return;
            Utilities.FlipTransform(transform);
            _facingRight = !_facingRight;
        }
    }

    public interface IPlayerController
    {
        public Vector2 Input { get; }
        public Vector2 Speed { get; }
        public bool Falling { get; }
        public bool Jumping { get; }
    }
}
