using UnityEngine;

public class Charactor : MonoBehaviour
{
    [SerializeField] string _charaData;  // 本来はCharaDataというScriptableObjectの型

    protected string CharaData => _charaData;
    public int HP;
    public float AttackRate;
    public float SpeedRate;

    public float ActSpan;  // 本来はCharaDataの一要素だが、仮でパラメータを用意
}
