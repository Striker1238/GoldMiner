using UnityEngine;

public static class SeedGenerator
{
    private static int seed;
    /// <summary>
    /// Сид от генератора случайных чисел
    /// </summary>
    public static int Seed { get => seed; private set { seed = value; Random.InitState(seed); Debug.Log($"New seed: {seed}"); } }
    static SeedGenerator() => RegenerateSeed();



    /// <summary>
    /// Генерирует новый сид и устанавливает его в Random.
    /// </summary>
    public static void RegenerateSeed() => Seed = GenerateSeed();
    private static int GenerateSeed() => (int)System.DateTime.Now.Ticks;
}