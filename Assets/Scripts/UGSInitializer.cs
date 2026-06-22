using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class UGSInitializer : MonoBehaviour
{
    async void Awake()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Debug.Log("UGS Initialized");
    }
}
