using UnityEngine;

public abstract class Item : ScriptableObject, IItem
{
    [SerializeField] protected int _id = -1;
    [SerializeField] protected string _name = "Empty";
    [SerializeField] protected string _description = "Empty";
    [SerializeField] protected Sprite _icon = null;
    [SerializeField] protected ItemRarity _rarity = ItemRarity.Common;
    [SerializeField] protected int _maxStack = 99;

    public int Id => _id;
    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
    public ItemRarity Rarity => _rarity;
    public int MaxStack => _maxStack;


    public override string ToString()
    {
        return $"{Name} (ID: {Id}, Rarity: {Rarity}, MaxStack: {MaxStack})";
    }
    public override bool Equals(object obj)
    {
        if (obj is IItem item)
        {
            return Id == item.Id;
        }
        return false;
    }
}
