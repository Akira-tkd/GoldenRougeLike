using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class StartButton : MonoBehaviour
{
    /*
     * ParrelSyncによる複数エディターでの接続テスト用
     * ボタンをクリックした時にホスト用ボタンか否かでNetworkManagerのStartメソッドを使い分ける
     */

    [SerializeField] private bool _isHost;  // このボタンがホスト用ボタンか否かを決めるbool

    public void OnClicked(string sceneName)
    {
        if (_isHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            // ローカル接続用の値設定
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = "127.0.0.1";
            transport.ConnectionData.Port = 7777;
            NetworkManager.Singleton.StartClient();
        }

        SceneManager.LoadScene(sceneName);
    }
}
