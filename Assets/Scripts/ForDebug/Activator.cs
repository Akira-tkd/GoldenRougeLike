using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] GameObject _item;

    public void OnClickedforActive()
    {
        _item.SetActive(true);
    }

    public void OnClickedforInactive()
    {
        _item.SetActive(false);
    }
}
