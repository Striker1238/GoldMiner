using UnityEngine;

public interface IInputSystem
{
    /// <summary>
    /// Метод описывающий движение персонажа в определенной системе управления
    /// </summary>
    Vector2 MovementDirection { get; }

    /// <summary>
    /// Метод взаимодействия персонажа с какими либо объектами
    /// </summary>
    bool InteractPressed { get; }
}

