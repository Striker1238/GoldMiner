using NUnit;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTest
{

    private ItemStub _potion;
    private ItemStub _sword;
    private ItemStub _arrow;
    private ItemStub _shield;

    [SetUp]
    public void Setup()
    {
        _potion = new ItemStub(1, "HealthPotion");
        _sword = new ItemStub(2, "IronSword");
        _arrow = new ItemStub(3, "Arrow");
        _shield = new ItemStub(4, "WoodenShield");
    }

    [TestCase(3)]
    [TestCase(99)]
    [TestCase(100)]
    public async Task AddItem_TryingAddCorrectItemQuantity_WhenInventoryHasSpace(int amount)
    {
        // Arrange
        var inv = new Inventory("Player", 5);
        inv.Slots.ForEach(s => s.Unlock());

        // Act
        await inv.AddItem(_potion, amount);

        // Assert
        Assert.That(await inv.ContainsItem(_potion), Is.EqualTo(amount > 0));
        Assert.That(await inv.GetCount(_potion), Is.EqualTo(Math.Max(0, amount)));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public async Task AddItem_TryingAddIncorrectItemQuantity_WhenInventoryHasSpace(int amount)
    {
        // Arrange
        var inv = new Inventory("Player", 5);
        inv.Slots.ForEach(s => s.Unlock());

        // Act & Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await inv.AddItem(_potion, amount));
    }

    [TestCase(3)]
    public async Task AddItem_TryingAddNullItem_WhenInventoryHasSpace(int amount)
    {
        // Arrange
        var inv = new Inventory("Player", 5);
        inv.Slots.ForEach(s => s.Unlock());

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await inv.AddItem(null, amount));
    }

    [TestCase(3)]
    [TestCase(99)]
    [TestCase(100)]
    public async Task AddItem_TryingAddCorrectItemQuantity_WhenInventoryNoSpace(int amount)
    {
        // Arrange
        var inv = new Inventory("Player", 1);
        inv.Slots.ForEach(s => s.Unlock());
        await inv.AddItem(_sword, 99);

        // Act
        // Assert
        Assert.That(await inv.GetCount(_sword), Is.EqualTo(99));

        Assert.ThrowsAsync<InvalidOperationException>(async () => await inv.AddItem(_potion, amount));
    }

    [TestCase(4)]
    [TestCase(98)]
    public async Task RemoveItem_TryingRemoveCorrectItemQuantity_WhenInventoryHasItem(int amount)
    {
        // Arrange
        var inv = new Inventory("Player", 5);
        inv.Slots.ForEach(s => s.Unlock());

        // Act
        await inv.AddItem(_potion, 99);
        Assert.That(await inv.GetCount(_potion), Is.EqualTo(99));

        await inv.RemoveItem(_potion, amount);
        // Assert
        Assert.That(await inv.ContainsItem(_potion), Is.True);
        Assert.That(await inv.GetCount(_potion), Is.EqualTo(99 - amount));
    }

    [TestCase(2)]
    [TestCase(99)]
    public async Task RemoveItem_TryingRemoveCorrectItemQuantity_WhenInventoryHasNotItemOrNoCorrectQuantity(int amount)
    {
        // Arrange
        var inv = new Inventory("Player", 5);
        inv.Slots.ForEach(s => s.Unlock());

        // Act
        await inv.AddItem(_potion, 1);
        Assert.That(await inv.GetCount(_potion), Is.EqualTo(1));

        // Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await inv.RemoveItem(_potion, amount));
        Assert.That(await inv.ContainsItem(_potion), Is.True);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task SortItemsById_MergesSameItemsIntoOne_WhenSortBy(int typeSort)
    {
        // Arrange
        var inv = new Inventory("Player", 5);
        inv.Slots.ForEach(s => s.Unlock());

        await inv.AddItem(_shield, 1);
        await inv.AddItem(_shield, 3);
        await inv.AddItem(_potion, 2);
        await inv.AddItem(_sword, 1);



        // Act
        switch (typeSort)
        {
            case 0:
                await inv.SortItemsById();
                break;
            case 1:
                await inv.SortItemsByCount(true);
                break;
            case 2:
                await inv.SortItemsByName();
                break;
            default:
                await inv.SortItemsById();
                break;
        }

        // Assert
        var nonEmpty = inv.Slots.Where(s => s.Item is not null).ToList();

        Assert.That(nonEmpty.Count == 3, Is.True);
        switch (typeSort)
        {
            case 0:
                Assert.That(nonEmpty[0].Item.Id == 1, Is.True);
                Assert.That(nonEmpty[1].Item.Id == 2, Is.True);
                Assert.That(nonEmpty[2].Count == 4, Is.True); // 1+3=4
                break;
            case 1:
                Assert.That(nonEmpty[0].Item.Id == 4, Is.True);
                Assert.That(nonEmpty[1].Item.Id == 1, Is.True);
                Assert.That(nonEmpty[2].Item.Id == 2, Is.True);
                break;
            case 2:
                Assert.That(nonEmpty[0].Item.Id == 1, Is.True);
                Assert.That(nonEmpty[1].Item.Id == 2, Is.True);
                Assert.That(nonEmpty[2].Item.Id == 4, Is.True);
                break;
            default:
                break;
        }
    }
}
