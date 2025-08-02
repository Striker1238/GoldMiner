using UnityEngine;

public class PlayerStateMachine
{
    private IPlayerState currentState;
    public virtual IPlayerState CurrentState => currentState;
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Tick()
    {
        currentState?.Update();
    }
}
