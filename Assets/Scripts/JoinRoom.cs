using UnityEngine;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class JoinRoom : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _title;
    Lobby _lobby;

    public void SetLobby(Lobby lobby)
    {
        _lobby = lobby;

        string text = _lobby.Name;
        text += "\n" + _lobby.Players.Count.ToString() + "/" + _lobby.MaxPlayers.ToString();

        _title.text = text;
    }

    public async void OnClicked()
    {
        string joinCode = _lobby.Data["JoinCode"].Value;
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayData = allocation.ToRelayServerData("dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayData);
        NetworkManager.Singleton.StartClient();
    }

    public void DebugOnClicked()
    {
        Debug.Log("‰Ÿ‚³‚ê‚Ü‚µ‚½");
    }
}
