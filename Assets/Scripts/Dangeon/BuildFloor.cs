using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BuildFloor : MonoBehaviour
{
    [SerializeField] Tilemap _floorMap;
    [SerializeField] Tilemap _wallMap;

    [SerializeField] TileBase _floorTile;
    [SerializeField] TileBase _wallTile;

    [SerializeField] DangeonGenerator _dg;
    public void Build()
    {
        var map = _dg.GetMap();
        int width = map[0].Count;
        int height = map.Count;

        _floorMap.ClearAllTiles();
        _wallMap.ClearAllTiles();

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                if (map[y][x].IsWall)
                {
                    _wallMap.SetTile(pos, _wallTile);
                }
                else if (map[y][x].IsWater)
                {
                    Debug.Log("…˜H(ŽÀ‘•—\’è)");
                }
                else
                {
                    _floorMap.SetTile(pos, _floorTile);
                }
            }
        }
    }
}
