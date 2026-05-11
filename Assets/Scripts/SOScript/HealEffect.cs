using UnityEngine;

[CreateAssetMenu(fileName = "NewHealEffect", menuName = "Effect/HealEffect")]
public class HealEffect : Effect
{
    public int HealAmount;
    public Target HealTarget;

    public override void Execute(Charactor user, Charactor other)
    {
        switch (HealTarget)
        {
            case Target.User:
                user.HP += HealAmount;
                // ‚±‚±‚ÉÅ‘åHP‚ğ’´‚¦‚½ê‡‚Ìˆ—‚ğ‹L“ü
                break;
            case Target.Hitted:
                other.HP += HealAmount;
                break;
            case Target.All:
                user.HP += HealAmount;
                other.HP += HealAmount;
                break;
        }
    }
}
