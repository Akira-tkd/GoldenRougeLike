using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class OutputRoomList : MonoBehaviour
{
    [SerializeField] GameObject _roomButtonObject;
    [SerializeField] Transform _parentObject;

    public async void OnClicked()
    {
        QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync();
        foreach(Lobby lobby in response.Results)
        {
            GameObject button = Instantiate(_roomButtonObject, _parentObject);
            button.GetComponent<JoinRoom>().SetLobby(lobby);
        }
    }
}
