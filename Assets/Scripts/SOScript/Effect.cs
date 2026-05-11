using UnityEngine;

public enum Target
{
    User,
    Hitted,
    All,
    Other
}

public abstract class Effect : ScriptableObject
{
    public abstract void Execute(Charactor user, Charactor other);
}
