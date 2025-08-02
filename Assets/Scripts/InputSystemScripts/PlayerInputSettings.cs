using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSettings", menuName = "Config/Player Input Settings")]
public class PlayerInputSettings : ScriptableObject
{
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public float moveSpeed = 5f;
    public bool allowDiagonal = true;
    public bool normalizeMovement = true;
}
