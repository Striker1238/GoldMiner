using UnityEngine;

public class IdleState : IPlayerState
{
    public void Enter() => Debug.Log("Enter: Idle");
    public void Update() { /* ничего не делаем */ }
    public void Exit() => Debug.Log("Exit: Idle");
}
