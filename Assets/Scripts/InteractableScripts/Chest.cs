using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public void Interact(PlayerStateController player)
    {
        Debug.Log("Сундук открыт!");
    }
}