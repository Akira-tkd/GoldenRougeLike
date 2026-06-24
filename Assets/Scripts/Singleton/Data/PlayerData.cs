using UnityEngine;
using Unity.Netcode;

public class PlayerData
{
    public ulong PlayerId;
    public Vector2Int Position;
    public NetworkVariable<int> HP = new NetworkVariable<int>();
    public float NextActionTime;
    public float ActionSpan;
    public PlayerView PlayerView;
}
