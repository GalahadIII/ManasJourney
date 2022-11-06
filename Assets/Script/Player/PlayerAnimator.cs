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
            _anim = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();
            
            _player = GetComponentInParent<IPlayerController>();
        }
        
        void Update()
        {
            getCurrentState();
            
            if (_state == _lastState) return;
            _anim.Play(_state);
            _lastState = _state;
        }

        private void getCurrentState()
        {
            if (Mathf.Abs(_player.Input.x) > 0)
            {
                _state = PlayerAnimState.RUN;
            }
            else
            {
                _state = PlayerAnimState.IDLE_BLINK;
            }

            if (_player.Falling && !_player.Grounded)
            {
                _state = PlayerAnimState.Fall;
            }
            if (_player.Jumping && !_player.Grounded)
            {
                _state = PlayerAnimState.JUMP;
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
        public const string PRE_JUMP = "BeforeJump";
        public const string Fall = "Fall";
        
        // attack
        public const string ATTACK1 = "Attack1";
    }
}
