using UnityEngine;

public class CommunicationState : IPlayerState
{
    private readonly PlayerStateController player;
    private readonly NPC target;
    public CommunicationState(PlayerStateController player, NPC target)
    {
        this.player = player;
        this.target = target;
    }
    public void Enter()
    {
        Debug.Log("Enter: Communication");
        player.DisableControl();
        player.dialogueComponent.InitializeNewDialogue(target.Name, target.Dialogue);
    }
    public void Update() { }

    public void Exit() 
    {
        Debug.Log("Exit: Communication");
        player.EnableControl();
    }
}