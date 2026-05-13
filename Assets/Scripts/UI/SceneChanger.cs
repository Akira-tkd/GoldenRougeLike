using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string _sceneName;

    public void OnClicked()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
