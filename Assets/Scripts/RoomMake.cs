using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System.Collections.Generic;

public class RoomMake : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tmp;

    public async void OnClicked()
    {
        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(_tmp.text, 4);
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        Debug.Log(joinCode);

        var relayData = allocation.ToRelayServerData("dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayData);

        await LobbyService.Instance.UpdateLobbyAsync(
            lobby.Id,
            new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {
                        "JoinCode",
                        new DataObject(DataObject.VisibilityOptions.Member,joinCode)
                    }
                }
            });

        NetworkManager.Singleton.StartHost();
    }
}
