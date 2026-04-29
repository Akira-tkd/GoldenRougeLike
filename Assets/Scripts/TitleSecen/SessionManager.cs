using UnityEngine;

public class SessionManager : MonoBehaviour
{
    /*
     * joincodeをシーンを跨いでも保持しておくためのクラス
     * シーンを跨がないといけないためグローバルアクセスポイントに
     * 今後セッションの設定をカスタムするようになった場合一部変わる可能性あり
     */

    public static SessionManager Instance { get; private set; }

    public string JoinCode;

    void Awake()
    {
        // シングルトンのためインスタンスの複数存在を回避
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // シーンを跨いでも保持されるための文
    }
}
