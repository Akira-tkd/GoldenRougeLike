using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class ConnectionRuner : MonoBehaviour
{
    /*
     * ButtonのOnClickedでは返り値のあるメソッドを呼べないため
     * 間に挟まることでButtonによる呼び出しを行える様にするためのクラス
     */

    [SerializeField] private ConnectwithRelay _CWR;  // 呼び出すメソッドが書かれているクラス
    [SerializeField] private TextMeshProUGUI _inputField;  // joincodeが入力されているテキスト

    public async void HostConnect()
    {
        bool connect = await _CWR.StartHostWithRelay();
        if (connect)
        {
            SceneManager.LoadScene("NetTest");  // 遷移先のシーン名が手打ちなのはよろしくない(Serializeで設定できるようにすべき)
        }
    }

    public async void ClientConnect()
    {
        string joinCode = _inputField.text;
        joinCode = Regex.Replace(joinCode, "[^A-Za-z0-9]","");  // 無駄な改行やゼロ幅スペースを消去
        bool connect = await _CWR.StartClientWithRelay(joinCode);

        if (connect)
        {
            SceneManager.LoadScene("NetTest");
        }
    }
}
