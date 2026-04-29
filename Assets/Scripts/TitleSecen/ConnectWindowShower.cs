using UnityEngine;

public class ConnectWindowShower : MonoBehaviour
{
    /*
     * サーバーを立てる、あるいは参加するためのUIを表示する処理が書かれたクラス
     * ButtonコンポーネントのOnClickedに割り当てて呼び出されることを想定している
     */

    [SerializeField] private GameObject _connectWindowCanvas;
    [SerializeField] private GameObject _hostUIObjects;
    [SerializeField] private GameObject _clientUIObjects;

    public void OnClickedHostButton()
    {
        _connectWindowCanvas.SetActive(true);
        _hostUIObjects.SetActive(true);
        _clientUIObjects.SetActive(false);
    }

    public void OnClickedClientButton()
    {
        _connectWindowCanvas.SetActive(true);
        _clientUIObjects.SetActive(true);
        _hostUIObjects.SetActive(false);
    }
}
