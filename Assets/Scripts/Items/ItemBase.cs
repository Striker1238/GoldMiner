using UnityEngine;

public abstract class ItemBase : ScriptableObject, IItem
{
    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemRarity rarity;
    [SerializeField] private int maxStack = 99;

    public string Id => id;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public ItemRarity Rarity => rarity;
    public int MaxStack => maxStack;
}
