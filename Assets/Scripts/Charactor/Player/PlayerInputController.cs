using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System.Collections.Generic;

public class PlayerInputController : NetworkBehaviour
{
    /*
     * プレイヤーの入力に関する処理を一括で管理するスクリプト
     * 別スクリプトの処理を呼び出す、ということもよくある
     */
    [Header("アタッチスクリプト欄")]
    [SerializeField] Player _player;
    [SerializeField] GridMoving _gridMoving;

    private bool _canAction;
    private float _actTimer;

    private Vector2Int _intVector;  // 入力方向を保存するための変数

    public void OnMove(InputAction.CallbackContext context)
    {
        var inputVector = context.ReadValue<Vector2>();

        _intVector = new Vector2Int(Mathf.RoundToInt(inputVector.x), Mathf.RoundToInt(inputVector.y));
    }

    void Start()
    {
        _canAction = false;
        _actTimer = 0.0f;
        _gridMoving.OnMoved += MovedAction;
    }

    void Update()
    {
        // 本来はマルチプレイなので必要
        // 現状はテストシーンすら仕上がっていないのでまだシングルプレイ想定の環境
        // if(!IsOwner) return;

        if(!_canAction)
        {
            _actTimer += Time.deltaTime;
            if( _actTimer > _player.ActSpan)
            {
                _canAction = true;
            }
        }
        else
        {
            // 以下は移動に関する処理を行うための処理
            var directionMagnitude = _intVector.sqrMagnitude;
            Vector2Int resultPosition;
            if (directionMagnitude > 1.4)
            {
                resultPosition = _gridMoving.TryMove(true, _player.Position, _intVector);
            }
            else if (directionMagnitude > 0.1)
            {
                resultPosition = _gridMoving.TryMove(false, _player.Position, _intVector);
            }
            else
            {
                return;
            }

            Debug.Log(resultPosition);
            if (resultPosition.y < _player.Map.Count && resultPosition.x < _player.Map[0].Count)
            {
                _player.Position = resultPosition;
                _canAction = false;
                _actTimer = 0.0f;
            }
        }
    }

    public void MovedAction()
    {
        if (_player.Map[_player.Position.y][_player.Position.x].OnItem != null)
        {
            _player.GetItem();
        }
        else
        {
            Debug.Log("歩いた");
        }
    }
}
