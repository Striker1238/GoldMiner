using UnityEngine;

public class PlayerStateController
{
    private readonly PlayerStateMachine stateMachine = new();
    public readonly PlayerDialogueComponent dialogueComponent;
    private readonly Rigidbody2D rb;
    private readonly IInputSystem input;
    private readonly float speed;
    private readonly IdleState idleState;
    private readonly MoveState moveState;
    private bool _canControl = true;


    public PlayerStateController(Rigidbody2D rb, IInputSystem input, float speed, PlayerDialogueComponent dialogueComponent)
    {
        this.rb = rb;
        this.input = input;
        this.speed = speed;
        this.dialogueComponent = dialogueComponent;
        idleState = new IdleState();
        moveState = new MoveState(rb, input, speed);
    }

    public void ChangeStateIdle() => stateMachine.ChangeState(idleState);

    public void StartInteraction(IInteractable target)
    {
        stateMachine.ChangeState(new InteractState(this, target));
    }
    public void StartMining(MineableObject target)
    {
        stateMachine.ChangeState(new MiningState(this, target));
    }
    public void StartCommunication(NPC target)
    {
        stateMachine.ChangeState(new CommunicationState(this, target));
    }
    public void DisableControl() => _canControl = false;
    public void EnableControl() => _canControl = true;
    public void Tick()
    {
        if (stateMachine == null)
            return;

        if (_canControl)
        {
            var current = stateMachine.CurrentState;

            if (current == idleState || current == moveState)
            {
                Vector2 move = input.MovementDirection;

                if (move.sqrMagnitude > 0.01f)
                {
                    if (current != moveState)
                        stateMachine.ChangeState(moveState);
                }
                else
                {
                    if (current != idleState)
                        stateMachine.ChangeState(idleState);
                }
            }
        }

        stateMachine.Tick();
    }

}