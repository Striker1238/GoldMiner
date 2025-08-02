using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovementComponent : MonoBehaviour
{
    private IInputSystem input;
    private PlayerStateController controller;
    private Rigidbody2D rb;
    [SerializeField]private float speed;

    [Inject]
    public void Construct(IInputSystem input, PlayerInputSettings settings)
    {
        this.input = input;
        this.speed = settings.moveSpeed;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = new PlayerStateController(rb, input, speed);
    }

    void Start() => controller.ChangeStateIdle();

    private void FixedUpdate()
    {
        controller.Tick();
    }
    void Update()
    {
        if (input.InteractPressed)
        {
            Debug.Log("Interactable pressed");
            //TODO: изменить получение направления 
            Vector2 dir = input.MovementDirection;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.5f, LayerMask.GetMask("Interactable"));
            if (hit.collider && hit.collider.TryGetComponent(out IInteractable interactable))
                controller.StartInteraction(interactable);
        }
    }
}