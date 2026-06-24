using UnityEngine;

public class MapManager
{
    public Grid Grid;
    public TileData[,] Map;

    public bool IsWalkable(Vector2Int position)
    {
        if(Map.GetLength(0) > position.y && position.y >= 0 && Map.GetLength(1) > position.x && position.x >= 0)
        {
            TileData tileData = Map[position.y, position.x];
            if (tileData.IsWalkable && tileData.OnEnemyId < 0 && tileData.OnPlayerId < ulong.MaxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    public void PlayerMove(ulong id, Vector2Int previous, Vector2Int current)
    {
        Map[previous.y, previous.x].OnPlayerId = ulong.MaxValue;
        Map[current.y, current.x].OnPlayerId = id;
    }
}
