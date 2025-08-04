using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable, INPC
{
    [SerializeField] private List<DialogueLine> dialogue;
    [SerializeField] private string npcName;
    [SerializeField] private NPCType npcType;

    public string Name => npcName;
    public NPCType Type => npcType;
    public List<DialogueLine> Dialogue => dialogue;

    public void Interact(PlayerStateController player)
    {
        //player.StartInteraction(this);
        player.StartCommunication(this);

    }
}
