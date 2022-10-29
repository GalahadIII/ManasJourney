using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [Header("MOVEMENT")]
    [Tooltip("The player's capacity to gain horizontal speed")]
    public float Acceleration = 120;
    
    [Tooltip("The top horizontal movement speed")]
    public float MaxSpeed = 14;
    
    [Tooltip("The pace at which the player comes to a stop")]
    public float GroundDeceleration = 60;
    
    [Tooltip("Deceleration in air only after stopping input mid-air")]
    public float AirDeceleration = 30;
    
    [Tooltip("A constant downward force applied while grounded. Helps on vertical moving platforms and slopes"), Range(0f, -10f)]
    public float GroundingForce = -1.5f;
    
    [Header("JUMP")]
    [Tooltip("Enable double jump")]
    public bool AllowDoubleJump;

    [Tooltip("The immediate velocity applied when jumping")]
    public float JumpPower = 36;

    [Tooltip("The maximum vertical movement speed")]
    public float MaxFallSpeed = 40;

    [Tooltip("The player's capacity to gain fall speed")]
    public float FallAcceleration = 110;

    [Tooltip("The gravity multiplier added when jump is released early")]
    public float JumpEndEarlyGravityModifier = 3;

    [Tooltip("The fixed frames before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
    public int CoyoteFrames = 7;

    [Tooltip("The amount of fixed frames we buffer a jump. This allows jump input before actually hitting the ground")]
    public int JumpBufferFrames = 7;
    
    [Header("COLLISIONS")]
    [Tooltip("Set this to the layer your player is on")]
    public LayerMask PlayerLayer;

    [Tooltip("The detection distance for grounding and roof detection")]
    public float GrounderDistance = 0.1f;

    [Tooltip("Bounds for detecting walls on either side. Ensure it's wider than your vertical capsule collider")]
    public Vector2 WallDetectorSize = new(0.75f, 1.25f);

    [Header("EXTERNAL")] 
    [Tooltip("The rate at which external velocity decays")]
    public int ExternalVelocityDecay = 100;

}
