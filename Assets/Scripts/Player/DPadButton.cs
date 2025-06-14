using UnityEngine;
using UnityEngine.EventSystems;

public class DPadButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;

    private ButtonInput input;

    private void Start()
    {
        input = FindObjectOfType<ButtonInput>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (direction)
        {
            case Direction.Up:    input.SetInputY(1);  break;
            case Direction.Down:  input.SetInputY(-1); break;
            case Direction.Left:  input.SetInputX(-1); break;
            case Direction.Right: input.SetInputX(1);  break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (direction)
        {
            case Direction.Up:
            case Direction.Down:  input.SetInputY(0); break;

            case Direction.Left:
            case Direction.Right: input.SetInputX(0); break;
        }
    }
}