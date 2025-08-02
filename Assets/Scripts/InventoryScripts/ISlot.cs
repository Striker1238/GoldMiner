public interface ISlot
{
    int Index { get; set; }
    IItem Item { get; set; }
    SlotType Type { get; }
    int Count { get; set; }
    bool IsEmpty { get; }
    bool IsFull { get; }
    bool IsLocked { get; }
    void Clear();
    void Lock();
    void Unlock();
}