using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public struct TilePalette
{
    /// <summary>
    /// Bitmask
    /// 128  1   2
    /// 64  255  4
    /// 32  16   8
    /// 
    /// Соответствующие суммы
    /// 1,1+2,128+1,128+1+2 - только верх
    /// 64,64+32,64+128,32+64+128 - только лево
    /// </summary>
    /// 
    public TileBase topLeftTile, topTile, topRightTile;
    public TileBase leftTile, centerTile, rightTile; 
    public TileBase bottomLeftTile, bottomTile, bottomRightTile;

    public TileBase topLeftInnerCornerTile, topRightInnerCornerTile, bottomLeftInnerCornerTile, bottomRightInnerCornerTile;

    /// <summary>
    /// Получение тайла по битмаске
    /// </summary>
    /// <param name="bitmask">Bit маска, по которой будет возвращен корректный тайл, расчет происходит
    /// по 8ми битной маской с дополнительным 9ым тайлом под центральный тайл </param>
    /// <returns></returns>
    public TileBase GetTileByBitmask(int bitmask)
    {
        switch (bitmask)
        {
            // Верх
            case 1:
            case 1 + 2:
            case 128 + 1:
            case 128 + 1 + 2: return topTile;

            // Право
            case 4:
            case 4 + 2:
            case 4 + 8:
            case 2 + 4 + 8: return rightTile;

            // Низ
            case 16:
            case 16 + 8:
            case 16 + 32:
            case 8 + 16 + 32: return bottomTile;
            
            // Лево
            case 64:
            case 64 + 32:
            case 64 + 128:
            case 32 + 64 + 128: return leftTile;

            //Обратный угл
            case 2: return topRightInnerCornerTile;
            case 8: return bottomRightInnerCornerTile;
            case 32: return bottomLeftInnerCornerTile;
            case 128: return topLeftInnerCornerTile;

            //Левый верхний угл
            case 1 + 64:
            case 1 + 64 + 128:
            case 1 + 2 + 64:
            case 1 + 64 + 32:
            case 1 + 2 + 64 + 32:
            case 1 + 128 + 64 + 32:
            case 1 + 2 + 64 + 128:
            case 1 + 2 + 128 + 64 + 32: return topLeftTile;

            // Правый верхний угл
            case 1 + 4:
            case 1 + 4 + 2:
            case 1 + 128 + 4:
            case 1 + 4 + 8:
            case 1 + 128 + 4 + 8:
            case 1 + 2 + 4 + 8:
            case 1 + 128 + 2 + 4:
            case 128 + 1 + 2 + 4 + 8: return topRightTile;

            // Нижний правый угл
            case 4 + 16:
            case 4 + 8 + 16:
            case 4 + 16 + 32:
            case 4 + 2 + 16:
            case 4 + 2 + 16 + 32:
            case 4 + 8 + 16 + 32:
            case 4 + 2 + 8 + 16:
            case 2 + 4 + 8 + 16 + 32: return bottomRightTile;

            // Нижний левый угл
            case 16 + 64:
            case 16 + 32 + 64:
            case 16 + 64 + 128:
            case 16 + 8 + 64:
            case 16 + 8 + 64 + 128:
            case 16 + 32 + 64 + 128:
            case 16 + 8 + 32 + 64:
            case 8 + 16 + 32 + 64 + 128: return bottomLeftTile;

            default: return centerTile;
        }
    }
}