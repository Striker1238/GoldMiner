using UnityEngine;

public class GroundItem : MonoBehaviour, IInteractable
{
    public string itemId;

    public void Interact(PlayerStateController player)
    {
        Debug.Log($"Подобрали предмет: {itemId}");
        Destroy(gameObject);
    }
}