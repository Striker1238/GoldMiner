using UnityEngine;

public interface IInputSystem
{
    Vector2 MovementDirection { get; }
    bool InteractPressed { get; }
}

