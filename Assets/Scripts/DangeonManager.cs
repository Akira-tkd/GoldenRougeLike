using UnityEngine;
using System.Collections.Generic;

public class DangeonManager : MonoBehaviour
{
    public delegate void OnChangedDelegate(List<List<DangeonTile>> map);
    public static event OnChangedDelegate OnChanged;

    public static DangeonManager Instance { get; private set; }
    public List<List<DangeonTile>> Map { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void Changed()
    {
        OnChanged?.Invoke(Map);
    }

    public void MapSetter(List<List<DangeonTile>> map)
    {
        Map = map;
    }
}
