using UnityEngine;
using Unity.Netcode;

public class ConnectingChecker : MonoBehaviour
{
    /*
     * NetworkManagerによる接続に関するテスト用
     * 接続されているか、ホスト、サーバー、クライアントのどのロールなのかを判別できているかのテスト用
     */

    void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Host");
        }
        else if(NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Server");
        }
        else
        {
            Debug.Log("Client");
        }

        if (!NetworkManager.Singleton.IsConnectedClient)
        {
            Debug.Log("未接続");
        }
        Debug.Log(NetworkManager.Singleton.IsClient);
        Debug.Log(NetworkManager.Singleton.IsListening);

    }

    void Update()
    {
        Debug.Log(NetworkManager.Singleton.IsConnectedClient);
        if(NetworkManager.Singleton.IsConnectedClient)
        {
            gameObject.SetActive(false);
        }
    }
}
