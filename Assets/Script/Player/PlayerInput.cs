using UnityEngine;
using UnityEngine.InputSystem;
using Player;
// ReSharper disable NotAccessedField.Global

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    
    private PlayerInputAction _actions;
    private InputAction _move, _jump, _dash, _attack;

    private void Awake()
    {
        _actions = new PlayerInputAction();
        _move = _actions.Player.Move;
        _jump = _actions.Player.Jump;
        _dash = _actions.Player.Dash;
        _attack = _actions.Player.Attack;
    }

    private void Update()
    {
        FrameInput = Gather();
    }

    private FrameInput Gather()
    {
        return new FrameInput {
            JumpDown = _jump.WasPressedThisFrame(),
            JumpUp = _jump.WasReleasedThisFrame(),
            DashDown = _dash.WasPressedThisFrame(),
            AttackDown = _attack.WasPressedThisFrame(),
            Move = _move.ReadValue<Vector2>()
        };
    }
    
    private void OnEnable() => _actions.Enable();

    private void OnDisable() => _actions.Disable();
}


public struct FrameInput
{
    public Vector2 Move;
    public bool JumpDown;
    public bool JumpUp;
    public bool DashDown;
    public bool AttackDown;
}
