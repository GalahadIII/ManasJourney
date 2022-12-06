using UnityEngine;
using Player;
using UnityEngine.InputSystem;
// ReSharper disable NotAccessedField.Global

public class InputManager : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    
    private PlayerInputActions _actions;
    private InputAction _move, _jump, _dash, _attack;

    private void Awake()
    {
        _actions = new PlayerInputActions();
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
            Move = _move.ReadValue<Vector2>(),
            JumpDown = _jump.WasPressedThisFrame(),
            JumpUp = _jump.WasReleasedThisFrame(),
            DashDown = _dash.WasPressedThisFrame(),
            AttackDown = _attack.WasPressedThisFrame()
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
