using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class OutputRoomList : MonoBehaviour
{
    [SerializeField] GameObject _roomButtonObject;
    [SerializeField] float _xInit;
    [SerializeField] float _yInit;
    [SerializeField] float _xOffset;
    [SerializeField] float _yOffset;

    public async void OnClicked()
    {
        QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync();

        var x = _xInit;
        var y = _yInit;
        foreach(Lobby lobby in response.Results)
        {
            GameObject button = Instantiate(_roomButtonObject);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
            button.GetComponent<JoinRoom>().SetLobby(lobby);

            x += _xOffset;
            y += _yOffset;
        }
    }
}
