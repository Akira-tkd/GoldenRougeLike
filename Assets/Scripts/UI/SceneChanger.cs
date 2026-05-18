using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string _sceneName;

    public void OnClicked()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }

    void Awake()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            gameObject.SetActive(false);
        }
    }
}
