using System.Collections;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class CorridorGenerator
{
    /// <summary>
    /// Ширина коридора в клетках.
    /// </summary>
    private int corridorWidth = 1;

    public CorridorGenerator(int corridorWidth)
    {
        this.corridorWidth = corridorWidth;
    }

    /// <summary>
    /// Генерирует коридор между двумя точками с небольшим шумом.
    /// </summary>
    /// <param name="start">Начальная позиция</param>
    /// <param name="end">Конечная позиция</param>
    public void GenerateNoisyCorridor(Vector2Int start, Vector2Int end, ref Map map)
    {
        Debug.Log($"Generating noisy corridor from {start} to {end}");
        Vector2Int current = start;

        while (current != end)
        {

            if (Random.value < 0.5f)
            {
                current.x += (end.x > current.x) ? 1 : (end.x < current.x ? -1 : 0);
            }
            else
            {
                current.y += (end.y > current.y) ? 1 : (end.y < current.y ? -1 : 0);
            }

            int halfWidth = corridorWidth / 2;
            for (int dx = -halfWidth; dx <= halfWidth; dx++)
            {
                for (int dy = -halfWidth; dy <= halfWidth; dy++)
                {
                    int nx = current.x + dx;
                    int ny = current.y + dy;
                    if (map != null && nx >= 0 && nx < map.Width && ny >= 0 && ny < map.Height)
                        map.SetTileType(nx, ny, TileType.Floor);
                }
            }
        }
        Debug.Log($"Corridor generation completed");

    }
}