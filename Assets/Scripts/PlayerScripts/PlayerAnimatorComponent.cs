using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorComponent : MonoBehaviour
{
    private Animator animator;
    private IInputSystem input;

    [Inject]
    public void Construct(IInputSystem input)
    {
        this.input = input;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 dir = input.MovementDirection;
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
        animator.SetBool("IsMoving", dir.sqrMagnitude > 0.01f);
    }
}
