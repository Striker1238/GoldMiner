public static class SeedGenerator
{
    public static int GenerateSeed() => (int)System.DateTime.Now.Ticks;
}