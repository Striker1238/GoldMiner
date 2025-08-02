using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool/Pickaxe")]
public class PickaxeItem : ItemBase, IEquippableItem
{
    [SerializeField] private SlotType slotType = SlotType.Tool;

    public SlotType SlotType => slotType;

    public void Equip()
    {
        //player.ToolManager.Equip(this);
    }

    public void Unequip()
    {
        //player.ToolManager.Unequip();
    }
}
