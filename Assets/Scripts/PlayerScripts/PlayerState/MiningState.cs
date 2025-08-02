using UnityEngine;

public class MiningState : IPlayerState
{
    private readonly PlayerStateController controller;
    private readonly MineableObject target;
    private float timer = 0f;

    public MiningState(PlayerStateController controller, MineableObject target)
    {
        this.controller = controller;
        this.target = target;
    }

    public void Enter()
    {
        timer = 0f;
        controller.DisableControl();
        Debug.Log("Добыча началась");
    }

    public void Update()
    {
        timer += Time.deltaTime;
        Debug.Log($"Добыча: {timer}/{target.miningTime}");
        if (timer >= target.miningTime)
        {
            target.CompleteMining();
            controller.ChangeStateIdle();
        }
    }

    public void Exit()
    {
        Debug.Log("Добыча завершена");
        controller.EnableControl();
    }

}