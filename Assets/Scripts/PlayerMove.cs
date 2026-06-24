using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using DG.Tweening;

public class PlayerAction : NetworkBehaviour
{
    private Vector2 _inputDirection;
    private float _localNextActionTime = 0.0f;

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if(_inputDirection.sqrMagnitude > 0)
        {
            if (_localNextActionTime < Time.time)
            {
                Vector2Int direction = Vector2Int.CeilToInt(_inputDirection);
                MoveServerRpc(direction);
            }
        }
    }

    [ServerRpc(RequireOwnership=false)]
    void MoveServerRpc(Vector2Int direction, ServerRpcParams rpcParams = default)
    {
        ulong id = rpcParams.Receive.SenderClientId;
        PlayerData pData = GameServer.Instance.PlayerManager.Players[id];
        Vector2Int currentPosition = pData.Position + direction;

        if (pData.NextActionTime < Time.time)
        {
            if(GameServer.Instance.MapManager.IsWalkable(currentPosition))
            {
                GameServer.Instance.MapManager.PlayerMove(id, pData.Position, currentPosition);
                pData.Position = currentPosition;
                
            }
        }
    }

/*    [ClientRpc]
    void MoveMotion(Vector2Int previous, Vector2Int currnet)
    {
        transform.DOMove(currnet, 0.5f);
    }*/
}
