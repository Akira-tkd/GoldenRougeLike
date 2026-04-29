using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Multiplayer;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System;

public class ConnectwithRelay : MonoBehaviour
{
    void Awake()
    {
        // エディターでのテスト中に同一認証によるプレイヤー重複エラーを回避するための処理
#if UNITY_EDITOR
PlayerPrefs.DeleteAll();
#endif
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        try
        {
            // 儀式的な一文
            await UnityServices.InitializeAsync(); // Awake側に移しても問題ない可能性あり


            // 開発中の環境でプレイヤーかぶりを回避するための処理
#if UNITY_EDITOR
            if(AuthenticationService.Instance.IsSignedIn)
            {
                AuthenticationService.Instance.SignOut();
            }
#endif

            // 匿名アカウントで認証を突破するための文
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }


            // セッションへの参加を行う。
            var session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);

            NetworkManager.Singleton.StartClient();
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError($"接続失敗:{e.Message}");
            return false;
        }

    }

    public async Task<bool> StartHostWithRelay()
    {
        try
        {
            // 儀式的な一文
            await UnityServices.InitializeAsync();  // Awake側に移しても問題ない可能性あり

            // セッションの設定を決めれる。最大人数や公開設定、パスワードはここで決める
            var options = new SessionOptions
            {
                MaxPlayers = 15
            }.WithRelayNetwork();

            // 匿名アカウントで認証を突破するための文
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            var session = await MultiplayerService.Instance.CreateSessionAsync(options);


            SessionManager.Instance.JoinCode = session.Code;

            NetworkManager.Singleton.StartHost();
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError($"ホスト接続失敗:{e.Message}");
            return false;
        }

    }
}
