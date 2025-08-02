using System.Collections.Generic;
using System.Linq;

public class Inventory : IStorage
{
    private string name;
    private int capacity;
    private List<ISlot> slots;

    public Inventory() : this("Default Inventory", 3) {}
    public Inventory(string name, int capacity)
    {
        this.name = name;
        this.capacity = capacity;
        slots = new List<ISlot>(capacity);
        for (int i = 0; i < capacity; i++)
        {
            slots.Add(new Slot(i));
        }

    }
    public string Name => name;

    public int Capacity => capacity;

    public List<ISlot> Slots => slots;

    public void AddItem(IItem item, int amount)
    {
        if (item == null)
        {
            throw new System.ArgumentNullException(nameof(item), "Item cannot be null.");
        }
        // Тут проблема есть в том, что при добавлении количества предметов, он может выйти за границу макс.стека
        var existingSlot = slots.FirstOrDefault(slot => slot.Item != null && slot.Item.Id == item.Id);
        if (existingSlot != null)
        {
            existingSlot.Count += amount;
            return;
        }

        var emptySlot = slots.FirstOrDefault(slot => slot.IsEmpty);
        if (emptySlot != null)
        {
            emptySlot.Item = item;
            emptySlot.Count += amount;
            return;
        }

        throw new System.InvalidOperationException("No available slot for the item.");
    }

    public void ClearStorage() => slots.ForEach(slot => slot.Clear());


    public bool ContainsItem(IItem item)
    {
        if (item == null)
        {
            throw new System.ArgumentNullException(nameof(item), "Item cannot be null.");
        }
        return slots.Any(slot => slot.Item != null && slot.Item.Id == item.Id);
    }

    public int GetCount(IItem item) => 
        slots.Where(slot => slot.Item != null && slot.Item.Id == item.Id)
            .Sum(slot => slot.Count);

    public IItem GetItemById(string id) =>
        slots.FirstOrDefault(slot => slot.Item != null && slot.Item.Id == id)?.Item;

    public void RemoveItem(IItem item, int amount)
    {
        if (item == null)
        {
            throw new System.ArgumentNullException(nameof(item), "Item cannot be null.");
        }

        if (GetCount(item) < amount)
        {
            throw new System.InvalidOperationException("Not enough items to remove.");
        }
        var allSlots = slots.Where(slot => slot.Item != null && slot.Item.Id == item.Id).ToList();
        int remainingToRemove = amount;
        foreach (var slot in allSlots)
        {
            if (remainingToRemove <= 0) break;
            if (slot.Count > remainingToRemove)
            {
                slot.Count -= remainingToRemove;
                remainingToRemove = 0;
            }
            else
            {
                remainingToRemove -= slot.Count;
                slot.Clear();
            }
        }
    }

    public void SortItems()
    {
        throw new System.NotImplementedException();
    }
}