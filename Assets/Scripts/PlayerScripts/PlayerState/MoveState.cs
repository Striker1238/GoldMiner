using UnityEngine;

public class MoveState : IPlayerState
{
    private readonly Rigidbody2D rb;
    private readonly IInputSystem input;
    private readonly float speed;

    public MoveState(Rigidbody2D rb, IInputSystem input, float speed)
    {
        this.rb = rb;
        this.input = input;
        this.speed = speed;
    }

    public void Enter() => Debug.Log("Enter: Move");

    public void Update()
    {
        Vector2 move = input.MovementDirection;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    public void Exit() => Debug.Log("Exit: Move");
}
