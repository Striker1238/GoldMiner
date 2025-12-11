using System.Threading.Tasks;
using UnityEngine;
public class Slot : ISlot
{
    [SerializeField] private int _index;
    [SerializeField] private IItem _item;
    [SerializeField] private SlotType _slotType;
    [SerializeField] private int _count;
    [SerializeField] private bool _isLocked;


    public Slot() : this(-1) { }
    public Slot(int index)
    {
        _index = index;
        _item = null;
        _slotType = SlotType.None;
        _count = 0;
        _isLocked = true;
    }

    public int Index { 
        get => _index; 
        set => _index = value; 
    }
    public IItem Item { 
        get => _item; 
        set => _item = value; 
    }

    public SlotType Type => _slotType;

    public int Count { 
        get => _count; 
        set => _count = value; 
    }

    public bool IsLocked => _isLocked;

    public void Clear()
    {
        _item = null;
        _count = 0;
    }
    public void Lock() => _isLocked = true;

    public void Unlock() => _isLocked = false;
}