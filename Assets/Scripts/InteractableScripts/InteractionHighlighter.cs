using UnityEngine;
using UnityEngine.UI;

public class InteractionHighlighter : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float interactRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Outline")]
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineThickness = 1.5f;

    [Header("UI")]
    [SerializeField] private GameObject interactionIconPrefab;
    [SerializeField] private Vector3 iconOffset = new Vector3(0, 1.2f, 0);
    
    private GameObject interactionIconInstance;
    private GameObject currentTarget;
    private Outline currentOutline;

    void FixedUpdate()
    {
        DetectInteractable();
    }

    private void DetectInteractable()
    {
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, interactRadius, interactableLayer);

        if (targetCollider != null && targetCollider.gameObject != currentTarget)
        {
            ResetPrevious();

            currentTarget = targetCollider.gameObject;

            currentOutline = currentTarget.GetComponent<Outline>();
            if (currentOutline == null)
            {
                currentOutline = currentTarget.AddComponent<Outline>();
            }

            currentOutline.effectColor = outlineColor;
            currentOutline.effectDistance = new Vector2(outlineThickness, outlineThickness);
            currentOutline.enabled = true;

            ShowInteractionIcon(currentTarget.transform);
        }
        else if (targetCollider == null)
        {
            ResetPrevious();
        }
    }

    private void ResetPrevious()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }

        if (interactionIconInstance != null)
        {
            Destroy(interactionIconInstance);
        }

        currentTarget = null;
    }

    private void ShowInteractionIcon(Transform target)
    {
        if (interactionIconPrefab == null) return;

        interactionIconInstance = Instantiate(interactionIconPrefab, target.position + iconOffset, Quaternion.identity);
        interactionIconInstance.transform.SetParent(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
