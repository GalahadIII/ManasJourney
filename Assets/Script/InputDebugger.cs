using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebugger : MonoBehaviour
{
    private void Update()
    {
    }
    
    private void OnJump(InputValue value)
    {
        Debug.Log("JumpInput");
    }

    private void OnMove(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>();
        Debug.Log("MoveInput " + movementVector);
    }
}
