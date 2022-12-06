using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour
    {
        private IPlayerController _player;
        private Animator _anim;
        private SpriteRenderer _sr;

        private string _state;
        private string _lastState;


        void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
            _player = GetComponentInParent<IPlayerController>();
        }
        
        void Update()
        {
            getCurrentState();
            
            if (_state == _lastState) return;
            //Debug.Log(_state);
            _anim.Play(_state);
            _lastState = _state;
        }

        private void getCurrentState()
        {
            if (_player.Input.x == 0)
            {
                _state = PlayerAnimState.IDLE_BLINK;
            }
            if (Mathf.Abs(_player.Input.x) > 0)
            {
                _state = PlayerAnimState.RUN;
            }
            if (_player.Falling && !_player.Grounded)
            {
                _state = PlayerAnimState.FALL;
            }
            if (_player.Jumping && !_player.Grounded)
            {
                _state = PlayerAnimState.JUMP;
            }
            if (_player.Rolling)
            {
                _state = PlayerAnimState.ROLL;
            }
            if (_player.Dashing)
            {
                _state = PlayerAnimState.DASH;
            }
        }
    }

    public static class PlayerAnimState
    {
        // idle
        public const string IDLE = "Idle";
        public const string IDLE_BLINK = "IdleBlink";
        
        // movement
        public const string RUN = "Run";
        public const string JUMP = "Jump";
        public const string DASH = "Dash";
        public const string PRE_JUMP = "BeforeJump";
        public const string FALL = "Fall";
        public const string ROLL = "Roll";
        
        // attack
        public const string ATTACK1 = "Attack1";
    }
}
