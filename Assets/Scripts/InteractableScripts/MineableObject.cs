using UnityEngine;

public class MineableObject : MonoBehaviour, IInteractable
{
    public string resourceType;
    public float miningTime = 2f;

    public void Interact(PlayerStateController player)
    {
        player.StartMining(this);
    }

    public void CompleteMining()
    {
        Debug.Log($"Добыли {resourceType}");
        Destroy(gameObject);
    }
}