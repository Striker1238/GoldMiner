using UnityEngine;

/// <summary>
/// Статический класс для генерации и хранения сида
/// </summary>
public static class SeedGenerator
{
    private static int seed;
    /// <summary>
    /// Сид от генератора случайных чисел
    /// </summary>
    public static int Seed { 
        get => seed; 
        private set {
            seed = value; 
            Random.InitState(seed); 
            Debug.Log($"Generate new seed: {seed}"); 
        } 
    }
    static SeedGenerator() 
    {
        Debug.Log($"Initializing SeedGenerator...");
        RegenerateSeed();
    }



    /// <summary>
    /// Пересоздает seed и устанавливает его в Random
    /// </summary>
    public static void RegenerateSeed() => Seed = GenerateSeed();

    /// <summary>
    /// Генерирует новый сид на основе текущего времени
    /// </summary>
    /// <returns>Возвращает новый seed</returns>
    private static int GenerateSeed() => (int)System.DateTime.Now.Ticks;
}