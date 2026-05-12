using UnityEngine;
using System.Collections.Generic;

public class DangeonManager : MonoBehaviour
{

    public static DangeonManager Instance { get; private set; }
    public List<List<DangeonTile>> Map { get; private set; }
    public Dictionary<Vector2Int, GameObject> ItemObjectDict { get; private set; }

    void Awake()
    {
        Instance = this;
        ItemObjectDict = new Dictionary<Vector2Int, GameObject>();
    }

    public void DeleteItem(Vector2Int pos)
    {
        if(ItemObjectDict.TryGetValue(pos, out GameObject item))
        {
            Destroy(item);
        }
    }

    public void MapSetter(List<List<DangeonTile>> map)
    {
        Map = map;
    }
}
