using System.Collections.Generic;

public interface IStorage
{
    string Name { get; }
    int Capacity { get; }
    List<ISlot> Slots { get; }
    void AddItem(IItem item, int amount);
    void RemoveItem(IItem item, int amount);
    bool ContainsItem(IItem item);
    int GetCount(IItem item);
    IItem GetItemById(string id);
    void ClearStorage();
    void SortItems();

}