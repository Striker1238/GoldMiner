using UnityEngine;

public class InteractState : IPlayerState
{
    private readonly PlayerStateController controller;
    private readonly IInteractable target;

    public InteractState(PlayerStateController controller, IInteractable target)
    {
        this.controller = controller;
        this.target = target;
    }

    public void Enter()
    {
        Debug.Log("Enter: Interact");
        controller.ChangeStateIdle();
        target.Interact(controller);
    }

    public void Update() { }
    public void Exit() { }
}
