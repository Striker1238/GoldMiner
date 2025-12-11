using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopInput : IInputSystem
{
    private readonly PlayerInputSettings settings;

    public DesktopInput(PlayerInputSettings settings)
    {
        this.settings = settings;
    }

    
    public Vector2 MovementDirection
    {
        get
        {
            float moveX = Input.GetAxisRaw(settings.horizontalAxis);
            float moveY = Input.GetAxisRaw(settings.verticalAxis);

            if (!settings.allowDiagonal)
                moveY = moveX != 0 ? 0 : moveY;

            var move = new Vector2(moveX, moveY);
            return settings.normalizeMovement ? move.normalized : move;
        }
    }
    public bool InteractPressed => Input.GetKeyDown(KeyCode.E);
}
