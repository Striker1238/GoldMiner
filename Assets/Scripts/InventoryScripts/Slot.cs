using UnityEngine;
public class Slot : ISlot
{
    [SerializeField] private int index;
    [SerializeField] private int count;
    [SerializeField] private IItem item;
    [SerializeField] private SlotType type;
    [SerializeField] private bool isLocked;
    
    public Slot(int index, IItem item = null, SlotType type = SlotType.None, int count = 0, bool isLocked = false)
    {
        this.index = index;
        this.item = item;
        this.count = count;
        this.isLocked = isLocked;
        this.type = type;
    }

    public int Index { get => index; set => index = value; }
    public int Count { get => count; set => count = value; }
    public IItem Item
    {
        get => item;
        set
        {
            if (value == null)
            {
                count = 0;
            }
            item = value;
        }
    }
    public SlotType Type => type;
    public bool IsEmpty => item == null || count == 0;
    public bool IsFull => item.MaxStack <= count;
    public bool IsLocked => isLocked;

    public void Clear()
    {
        item = null;
        count = 0;
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }
}