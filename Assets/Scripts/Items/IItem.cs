using UnityEngine;

public interface IItem
{
    string Id { get; }
    string DisplayName { get; }
    Sprite Icon { get; }
    ItemRarity Rarity { get; }
    int MaxStack { get; }
}