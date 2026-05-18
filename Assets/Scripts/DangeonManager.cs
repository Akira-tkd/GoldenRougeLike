using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class DangeonManager : NetworkBehaviour
{

    public static DangeonManager Instance { get; private set; }
    public List<List<DangeonTile>> Map { get; private set; }

    public NetworkVariable<int> FloorSeed;

    public Dictionary<int, ItemObject> ItemObjectDict;
    public Dictionary<int, Item> ItemDict;
    public Dictionary<int, Player> PlayerDict;

    private int nextItemId;

    public override void OnNetworkSpawn()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += Init;
    }
    
    void Init(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Initスタート");
        if (NetworkManager.Singleton.IsHost)
        {
            FloorSeed.Value = Guid.NewGuid().GetHashCode();
        }

        ItemObjectDict = new Dictionary<int, ItemObject>();
        ItemDict = new Dictionary<int, Item>();
        nextItemId = 0;
        PlayerDict = new Dictionary<int, Player>();
    }

    public void AddItem(ItemObject itemObject, Item item)
    {
        item.Id = nextItemId;
        ItemObjectDict.Add(nextItemId, itemObject);
        ItemDict.Add(nextItemId, item);
        nextItemId++;
    }

    public void DeleteItem(int id)
    {
        Debug.Log($"受け取ったID：{id}");
        Debug.Log(ItemObjectDict.Count);
        if(ItemObjectDict.TryGetValue(id, out ItemObject item))
        {
            Debug.Log("削除開始");
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
