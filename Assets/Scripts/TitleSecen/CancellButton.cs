using UnityEngine;

public class CancellButton : MonoBehaviour
{
    /*
     * サーバー立て、参加をキャンセルするときに押すボタン
     * サーバー用に表示されたUIの元のキャンバスを非アクティブに変える
     */

    [SerializeField] private GameObject _canvas;
    public void OnClicked()
    {
        _canvas.SetActive(false);
    }
}
