using UnityEngine;
using Unity.Netcode;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    [SerializeField] GameObject _playerPrefab;

    public void OnClicked()
    {
        PlayerSpawnServerRpc();
    }

    [ServerRpc(RequireOwnership=false)]
    void PlayerSpawnServerRpc()
    {
        GameObject player = Instantiate(_playerPrefab);
        player.GetComponent<Player>().Init(new Vector2Int(0, 0), _playerData);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(1);
    }
}
