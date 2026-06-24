using UnityEngine;
using Unity.Netcode;
using DG.Tweening;

public class PlayerView : NetworkBehaviour
{

    private void MovingAnimation(Vector2Int previous, Vector2Int current)
    {
        if((current - previous).sqrMagnitude <= 2)
        {
            transform.DOMove(new Vector3(current.x, current.y, 0f), 1f);
        }
    }
}
