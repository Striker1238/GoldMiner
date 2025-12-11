using System.Threading.Tasks;
public interface ISlot
{
    /// <summary>
    /// Индекс слота в хранилище
    /// </summary>
    int Index { get; set; }
    /// <summary>
    /// Содержание слота
    /// </summary>
    IItem Item { get; set; }
    /// <summary>
    /// Тип слота
    /// </summary>
    SlotType Type { get; }
    /// <summary>
    /// Количество предметов в слоте
    /// </summary>
    int Count { get; set; }
    /// <summary>
    /// Заблокирован ли слот
    /// </summary>
    bool IsLocked { get; }
    /// <summary>
    /// Отчищает слот, удаляя предмет и обнуляя количество
    /// </summary>
    void Clear();
    /// <summary>
    /// Метод блокировки слота, переводит состояне IsLocked в true
    /// </summary>
    void Lock();
    /// <summary>
    /// Метод разблокировки слота, переводит состояние IsLocked в false
    /// </summary>
    void Unlock();
}