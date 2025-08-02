using UnityEngine;

public class MobileInput : IInputSystem
{
    public Vector2 MovementDirection => Vector2.zero; // TODO: заменить на UI джойстик
    public bool InteractPressed => Input.GetKeyDown(KeyCode.E);
}
