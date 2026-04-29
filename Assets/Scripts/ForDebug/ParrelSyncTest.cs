using UnityEngine;
using Unity.Netcode;
using ParrelSync;

public class ParrelSyncTest : MonoBehaviour
{
    /*
     * ParrelSyncによる複数エディターの運用テスト用
     * 接続状態がどうこう関係なくクローンエディターか否かを判別できるかのテスト
     */

    void Start()
    {
        if (ClonesManager.IsClone())
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Client");
        }
        else
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Host");
        }
    }
}
