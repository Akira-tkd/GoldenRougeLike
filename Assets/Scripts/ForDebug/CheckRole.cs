using UnityEngine;
using Unity.Netcode;

public class CheckRole : MonoBehaviour
{
    public void OnClicked()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Server");
        }
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Host");
        }
        if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Client");
        }
        Debug.Log("Pushed");
    }
}
