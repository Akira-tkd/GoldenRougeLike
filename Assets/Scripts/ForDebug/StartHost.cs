using UnityEngine;
using Unity.Netcode;

public class StartHost : MonoBehaviour
{
    public void OnClicked()
    {
        NetworkManager.Singleton.StartHost();
    }
}
