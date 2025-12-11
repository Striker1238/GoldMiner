public class SlotStub : ISlot
{
    public IItem Item { get; set; }
    public int Amount { get; private set; }

    public bool IsEmpty => Item == null;

    public int Index { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
     
    public SlotType Type => throw new System.NotImplementedException();

    public int Count { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public bool IsLocked => throw new System.NotImplementedException();

    public void Set(IItem item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public void Clear()
    {
        Item = null;
        Amount = 0;
    }

    public void Lock()
    {
        throw new System.NotImplementedException();
    }

    public void Unlock()
    {
        throw new System.NotImplementedException();
    }
}
