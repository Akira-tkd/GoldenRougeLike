using UnityEngine;

public class JoinCodeCopy : MonoBehaviour
{
    /*
     * joincodeをコピーするためのボタンにアタッチするクラス
     * joincodeはグローバルアクセスポイントであるSessionManagerがpublicなstringとして保持している
     */

    public void Copy()
    {
        GUIUtility.systemCopyBuffer = SessionManager.Instance.JoinCode;
        Debug.Log("コピーしました");  // デバッグログだけでなく、UIとして表示するように変更する必要あり
    }
}
