using NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : IStorage
{
    [SerializeField] private string _name;
    [SerializeField] private int _capacity;
    [SerializeField] private List<ISlot> _slots;

    public Inventory() : this("Default Inventory", 1) { }
    public Inventory(string name, int capacity)
    {
        _name = name;
        _capacity = capacity;
        _slots = new();

        for (int i = 0; i < capacity; i++) 
            _slots.Add(new Slot(i));

        Debug.Log($"Creating inventory '{_name}' with slots/capacity {_slots.Count}/{_capacity}");
    }

    public string Name => _name;
    public int Capacity => _capacity;
    public List<ISlot> Slots => _slots;
    public async Task AddItem(IItem item, int amount)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");

        if (_slots is null)
            throw new InvalidOperationException("Slots collection is not initialized");


        // 1. Ищем слоты с таким же предметом, не заблокированные и количество меньше чем фулл стак
        List<ISlot> availableSlots = _slots
            .Where(s => s.Item?.Equals(item) ?? false && !s.IsLocked && s.Count < (s.Item?.MaxStack ?? 99))
            .ToList();

        // 2. Ищем слоты, где предметы отсутствуют
        availableSlots.AddRange(_slots.Where(s => s.Item is null && !s.IsLocked).ToList());

        Debug.Log($"Total available slots for adding item {item.Name}: {availableSlots.Count}");

        // 3. Проверяем сколько доступно слотов и сколько можем поместить 
        if (availableSlots.Count <= 0 || availableSlots.Sum(s => (s.Item?.MaxStack ?? 99) - s.Count) < amount)
            throw new InvalidOperationException($"Not enough space in inventory to add the item.");


        // 4. Добавляем предметы в слоты
        foreach (var s in availableSlots)
        {
            int spaceLeft = item.MaxStack - s.Count;

            if (spaceLeft >= amount)
            {
                s.Item = item;
                s.Count += amount;
                amount = 0;
                break;
            }
            else
            {
                s.Item = item;
                s.Count += spaceLeft;
                amount -= spaceLeft;
            }
        }
        
    }

    public Task ClearStorage()
    {
        _slots.ForEach(s => s.Clear());
        return Task.CompletedTask;
    }

    public Task<bool> ContainsItem(IItem item)
    {
        return Task.FromResult(_slots.Any(s => s.Item?.Equals(item) == true));
    }

    public Task<bool> ContainsItem(IItem item, int amount)
    {
        return Task.FromResult(_slots.Where(s => s.Item?.Equals(item) == true).Sum(s => s.Count) >= amount);
    }

    public Task<bool> ContainsItem(int id)
    {
        return Task.FromResult(_slots.Any(s => s.Item?.Id == id));
    }

    public Task<bool> ContainsItem(int id, int amount)
    {
        return Task.FromResult(_slots.Where(s => s.Item?.Id == id).Sum(s => s.Count) >= amount);
    }

    public Task<int> GetCount(IItem item)
    {
        int totalCount = _slots
            .Where(s => s.Item?.Equals(item) == true)
            .Sum(s => s.Count);
        return Task.FromResult(totalCount);
    }

    public Task<IItem> GetItemBySlotId(int index)
    {
        return Task.FromResult(_slots.FirstOrDefault(s => s.Index == index)?.Item);
    }

    public Task RemoveItem(IItem item, int amount)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if(amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");

        if(!ContainsItem(item, amount).Result)
            throw new InvalidOperationException("Not enough items in inventory to remove the specified amount");


        _slots.Where(s => s.Item?.Equals(item) == true)
            .ToList()
            .ForEach(s =>
            {
                if (amount <= 0)
                    return;
                if (s.Count > amount)
                {
                    s.Count -= amount;
                    amount = 0;
                }
                else
                {
                    amount -= s.Count;
                    s.Clear();
                }
            });
        return Task.CompletedTask;
    }


    public async Task SortItemsById()
    {
        await SortItemsBy(slot => slot.Item.Id);
    }

    public async Task SortItemsByCount(bool descending)
    {
        await SortItemsBy(slot => slot.Count, descending);
    }

    public async Task SortItemsByName()
    {
        await SortItemsBy(slot => slot.Item.Name);
    }


    private Task SortItemsBy(Func<ISlot, object> keySelector, bool descending = false)
    {
        MergeSameItems();

        //TODO: Это место не оптимально, каждый раз создаем новые слоты! Нужно поравить будет
        var itemsQuery = _slots
            .Where(s => !s.IsLocked && s.Item != null)
            .Select(s => new Slot()
            {
                Item = s.Item,
                Count = s.Count
            });

        var items = descending
            ? itemsQuery.OrderByDescending(keySelector).ToList()
            : itemsQuery.OrderBy(keySelector).ToList();

        foreach (var slot in _slots)
        {
            if (!slot.IsLocked)
                slot.Clear();
        }

        int index = 0;
        foreach (var data in items)
        {
            while (index < _slots.Count && _slots[index].IsLocked)
                index++;

            if (index >= _slots.Count)
                break;

            _slots[index].Item = data.Item;
            _slots[index].Count = data.Count;
            index++;
        }


        foreach (var slot in _slots)
        {
            Debug.Log($"Slot {slot.Index}: Item = {(slot.Item != null ? slot.Item.Name : "null")}, Count = {slot.Count}, IsLocked = {slot.IsLocked}");
        }
        return Task.CompletedTask;
    }
    /// <summary>
    /// Объединяет одинаковые предметы в инвентаре.
    /// </summary>
    private void MergeSameItems()
    {
        var merged = new Dictionary<int, ISlot>();

        foreach (var slot in _slots)
        {
            if (slot.IsLocked || slot.Item == null)
                continue;

            var id = slot.Item.Id;

            if (!merged.TryGetValue(id, out var mergedSlot))
            {
                merged[id] = slot;
            }
            else
            {
                mergedSlot.Count += slot.Count;
                slot.Clear();
            }
        }
    }

}