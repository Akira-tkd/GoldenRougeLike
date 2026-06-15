using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] Player _player;
    private Vector2 _inputDirection;

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if(_inputDirection.sqrMagnitude > 0)
        {
            if (_player.NextActionTime.Value < Time.time)
            {
                Vector2Int direction = Vector2Int.CeilToInt(_inputDirection);
                MoveServerRpc(direction);
            }
        }
    }

    [ServerRpc(RequireOwnership=false)]
    void MoveServerRpc(Vector2Int direction)
    {
        _player.Position.Value += direction;
        _player.NextActionTime.Value = Time.time + _player.ActionSpan;
    }
}
