using UnityEngine;

public class ButtonInput : MonoBehaviour, IPlayerInput
{
    private Vector2 currentInput;

    public void SetInputX(float x) => currentInput.x = x;
    public void SetInputY(float y) => currentInput.y = y;

    public Vector2 GetMovementInput() => currentInput;
}
