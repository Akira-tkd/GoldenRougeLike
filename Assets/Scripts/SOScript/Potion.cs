using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName="NewPotion", menuName="Item/Potion")]
public class Potion : Item
{
    [Header("ポーション用要素")]
    public List<Effect> UseEffects;
}
