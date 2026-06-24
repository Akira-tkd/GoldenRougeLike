using UnityEngine;
using Unity.Netcode;

public class GameServer : NetworkBehaviour
{
    public static GameServer Instance;

    public MapManager MapManager { get; private set; }

    public EnemyManager EnemyManager { get; private set; }

    public ItemManager ItemManager { get; private set; }

    public PlayerManager PlayerManager { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }


        PlayerManager = GameManager.Instance.PlayerManager;

        MapManager = new MapManager();
        EnemyManager = new EnemyManager();
        ItemManager = new ItemManager();
        
        Instance = this;
    }
}
