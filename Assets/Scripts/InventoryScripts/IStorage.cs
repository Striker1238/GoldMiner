using System.Collections.Generic;
using System.Threading.Tasks;

public interface IStorage
{
    /// <summary>
    /// Название хранилищя
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Размер хранилищя
    /// </summary>
    int Capacity { get; }
    /// <summary>
    /// Слоты в хранилище
    /// </summary>
    List<ISlot> Slots { get; }
    /// <summary>
    /// Добавить предмет в хранилище
    /// </summary>
    /// <param name="item">Предмет для добавления</param>
    /// <param name="amount">Количество добавляемых предметов</param>
    Task AddItem(IItem item, int amount);
    /// <summary>
    /// Удаление предмета из хранилища
    /// </summary>
    /// <param name="item">Предмет который будет удален из хранилищя</param>
    /// <param name="amount">Количество удаляемых предметов</param>
    Task RemoveItem(IItem item, int amount);
    /// <summary>
    /// Проверка, есть ли указанный предмет в хранилище
    /// </summary>
    /// <param name="item">Предмет для поиска</param>
    /// <returns>true - предмет находится в хранилище, false - предмет отсутствует</returns>
    Task<bool> ContainsItem(IItem item);
    /// <summary>
    /// Проверка, есть ли указанный предмет, в определенном размере, в хранилище
    /// </summary>
    /// <param name="item">Предмет для поиска</param>
    /// <param name="amount">Требуемое количество предметов</param>
    /// <returns>true - предмет находится в хранилище в нужном объеме, false - предмет отсутствует</returns>
    Task<bool> ContainsItem(IItem item, int amount);
    /// <summary>
    /// Проверка, есть ли указанный предмет в хранилище
    /// </summary>
    /// <param name="item">Индекс предмета для поиска</param>
    /// <returns>true - предмет находится в хранилище, false - предмет отсутствует</returns>
    Task<bool> ContainsItem(int id);
    /// <summary>
    /// Проверка, есть ли указанный предмет, в определенном размере, в хранилище
    /// </summary>
    /// <param name="item">Индекс предмета для поиска</param>
    /// <param name="amount">Требуемое количество предметов</param>
    /// <returns>true - предмет находится в хранилище в нужном объеме, false - предмет отсутствует</returns>
    Task<bool> ContainsItem(int id,int amount);
    /// <summary>
    /// Возвращает количество, указанного предмета, в хранилище 
    /// </summary>
    /// <param name="item">Предмет поиск которого происходит</param>
    /// <returns></returns>
    Task<int> GetCount(IItem item);
    /// <summary>
    /// Получаем предмет, который находится в указаном слоте
    /// </summary>
    /// <param name="id">Индекс слота</param>
    /// <returns>Объект предмета, по указанному слоту</returns>
    Task<IItem> GetItemBySlotId(int index);
    /// <summary>
    /// Отчищает все хранилище
    /// </summary>
    Task ClearStorage();
    /// <summary>
    /// Сортирует хранилище по id предметов
    /// </summary>
    Task SortItemsById();

}