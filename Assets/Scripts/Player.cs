using UnityEngine;
using Unity.Netcode;
using DG.Tweening;

public class Player : NetworkBehaviour
{
    public NetworkVariable<Vector2Int> Position = new NetworkVariable<Vector2Int>();
    public NetworkVariable<float> NextActionTime = new NetworkVariable<float>();
    public float ActionSpan { private set; get; }

    public void Init(Vector2Int startPosition, PlayerData playerData)
    {
        Position.Value = startPosition;
        NextActionTime.Value = 0.0f;
        ActionSpan = playerData.ActionSpan;
    }

    public override void OnNetworkSpawn()
    {
        Position.OnValueChanged += MovingAnimation;
    }

    private void MovingAnimation(Vector2Int previous, Vector2Int current)
    {
        if((current - previous).sqrMagnitude <= 2)
        {
            transform.DOMove(new Vector3(current.x, current.y, 0f), 1f);
        }
    }
}
