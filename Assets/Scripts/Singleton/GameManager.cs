using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public PlayerManager PlayerManager { get; private set; }

    public override void OnNetworkSpawn()
    {
        Instance = this;
        PlayerManager = new PlayerManager();
    }
}
