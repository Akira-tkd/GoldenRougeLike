using UnityEngine;
using System.Collections.Generic;

public class DangeonManager : MonoBehaviour
{

    public static DangeonManager Instance { get; private set; }
    public List<List<DangeonTile>> Map { get; private set; }
    public Dictionary<int, ItemObject> ItemObjectDict = new Dictionary<int, ItemObject>();
    public Dictionary<int, Item> ItemDict = new Dictionary<int, Item>();
    public Dictionary<int, Player> PlayerDict = new Dictionary<int, Player>();

    private int nextItemId = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemObject itemObject, Item item)
    {
        ItemObjectDict.Add(nextItemId, itemObject);
        ItemDict.Add(nextItemId, item);
        nextItemId++;
    }

    public void DeleteItem(int id)
    {
        if(ItemObjectDict.TryGetValue(id, out ItemObject item))
        {
            ItemObjectDict.Remove(id);
            ItemDict.Remove(id);
            Destroy(item.gameObject);
        }
    }

    public void MapSetter(List<List<DangeonTile>> map)
    {
        Map = map;
    }
}
