using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu]
    public class PlayerMoveStats : ScriptableObject
    {
        [Header("MOVEMENT")]
        public float MoveSpeed = 20f;
        
        public float Acceleration = 6f;
        
        public float Decceleration = 6f;

        public float VelPower = 1.2f;

        public float Friction = 0.7f;
        
        
        [Header("JUMP")]
        public float JumpForce = 5f;
        
        public float JumpCutMultiplier = 0.5f;
        
        public int JumpCoyoteFrame = 10;
        
        public int JumpBufferFrame = 10;

        public float FallGravityMultiplier = 1.9f;
        
    
        [Header("DASH")]
        public float DashVelocity = 50;
    }
}

