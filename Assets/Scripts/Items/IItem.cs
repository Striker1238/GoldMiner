using System.Threading.Tasks;
using UnityEngine;

public interface IItem
{
    /// <summary>
    /// Индентификатор предмета
    /// </summary>
    int Id { get; }
    /// <summary>
    /// Название предмета
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Описание предмета
    /// </summary>
    string Description { get; }
    /// <summary>
    /// Иконка предмета
    /// </summary>
    Sprite Icon { get; }
    /// <summary>
    /// Редкость предмета
    /// </summary>
    ItemRarity Rarity { get; }
    /// <summary>
    /// Максимальное количество предметов в стеке
    /// </summary>
    int MaxStack { get; }
}