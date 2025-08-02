using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private List<DialogueLine> dialogue;
    [SerializeField] private List<INPCAction> actions;

    public void Interact(PlayerStateController player)
    {
        player.StartInteraction(this);
    }

    public void Trigger(PlayerStateController controller)
    {
        controller.DialogueSystem.StartDialogue(dialogue, () =>
        {
            foreach (var action in actions)
                action.Execute(controller);
        });
    }
}
